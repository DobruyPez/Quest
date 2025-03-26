using UnityEngine;

public class TriggerTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMesh _textMesh;
    [SerializeField] private TriggerableTextData[] _triggerTextData;

    private void OnEnable()
    {
        foreach (var data in _triggerTextData)
        {
            if (data.trigger != null)
            {
                data.trigger.OnTriggered += HandleTriggered;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var data in _triggerTextData)
        {
            if (data.trigger != null)
            {
                data.trigger.OnTriggered -= HandleTriggered;
            }
        }
    }

    private void HandleTriggered(ITriggerable triggeredObject)
    {
        foreach (var data in _triggerTextData)
        {
            if (data.trigger == triggeredObject)
            {
                // Если триггер поддерживает ICheckableTrigger, проверяем его состояние
                if (triggeredObject is ICheckableTrigger checkableTrigger)
                {
                    _textMesh.text = checkableTrigger.IsDone ? data.doneText : data.defaultText;
                }
                else
                {
                    _textMesh.text = data.defaultText;
                }
                return;
            }
        }
        Debug.LogWarning($"No text data found for triggered object: {triggeredObject}", this);
    }

    [System.Serializable]
    public class TriggerableTextData
    {
        public ICheckableTrigger trigger;  // Триггер с проверкой состояния
        public string defaultText;        // Текст по умолчанию (если IsDone == false)
        public string doneText;          // Текст после выполнения (если IsDone == true)
    }
}