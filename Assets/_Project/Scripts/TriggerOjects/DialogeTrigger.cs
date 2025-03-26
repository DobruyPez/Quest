using System.Collections.Generic;
using UnityEngine;

internal class DialogeTrigger : MonoBehaviour, ITriggerable, ICheckableTrigger
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset _inkJSON;

    [Header("Dependent Triggers")]
    [SerializeField] private List<ITriggerable> _requiredTriggers = new List<ITriggerable>();

    public bool IsDone { get; private set; }

    private void Start()
    {
        FindAndAddRequiredTriggersByTag();
    }

    public void Trrigered()
    {
        if (CanTrigger())
        {
            DialogueManager dialogeManager = DialogueManager.GetInstance();
            dialogeManager.EnterDialogueMode(_inkJSON);
            IsDone = true;
        }
    }

    private bool CanTrigger()
    {
        if (IsDone) return false;

        foreach (var trigger in _requiredTriggers)
        {
            if (trigger == null) continue;

            // Если триггер реализует интерфейс с проверкой состояния
            if (trigger is ICheckableTrigger checkable && !checkable.IsDone)
            {
                return false;
            }
        }
        return true;
    }

    // Для отображения в инспекторе
    private void OnValidate()
    {
        // Конвертируем GameObject'ы в компоненты ITriggerable
        for (int i = 0; i < _requiredTriggers.Count; i++)
        {
            if (_requiredTriggers[i] is DialogeTrigger gameObj)
            {
                _requiredTriggers[i] = gameObj.GetComponent<DialogeTrigger>();
            }
        }
    }

    // Новый метод для поиска триггеров по тегу
    [ContextMenu("Find Required Triggers By Tag")]
    public void FindAndAddRequiredTriggersByTag()
    {
        if (transform.parent == null) return;

        // Получаем имя родителя, убираем "(clone)" если есть
        string parentName = transform.parent.name;
        parentName = parentName.Replace("(Clone)", "").Trim();

        // Формируем тег
        string searchTag = parentName + "RequiredTrigger";

        if (!TagExists(searchTag))
        {
            Debug.LogWarning($"Тег '{searchTag}' не существует. Проверьте настройки тегов в Project Settings.", this);
            return;
        }

        // Ищем все объекты с этим тегом
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(searchTag);

        if(taggedObjects != null)
        {
            //_requiredTriggers.Clear();

            foreach (GameObject obj in taggedObjects)
            {
                // Проверяем, чтобы не добавить себя самого
                if (obj == this.gameObject) continue;

                DialogeTrigger trigger = obj.GetComponent<DialogeTrigger>();
                if (trigger != null)
                {
                    _requiredTriggers.Add(trigger);
                }
            }

        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
    private bool TagExists(string tagName)
    {
        try
        {
            // Этот способ работает в Runtime и Editor
            GameObject.FindWithTag(tagName); // Если тег не существует, Unity выкинет ошибку
            return true;
        }
        catch (UnityException)
        {
            return false;
        }
    }
}