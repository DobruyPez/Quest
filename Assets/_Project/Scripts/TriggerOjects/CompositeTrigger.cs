using UnityEngine;
using System;
using System.Collections.Generic;

public class CompositeTrigger : MonoBehaviour, ICheckableTrigger
{
    [SerializeField] private List<ActionsAreCommitted> _requiredTriggers = new List<ActionsAreCommitted>();
    [SerializeField] private bool _once = false;

    public event Action OnActivated;
    public bool IsDone { get; private set; }

    private void Awake()
    {
        if (_requiredTriggers.Count == 0)
        {
            Debug.LogError($"No triggers assigned to CompositeTrigger on {gameObject.name}", this);
        }
    }

    public void Trrigered()
    {
        if (IsDone && _once) return; // Если уже активирован, не проверяем дальше

        bool allDone = true;
        foreach (var trigger in _requiredTriggers)
        {
            if (trigger == null)
            {
                Debug.LogError("Null trigger in CompositeTrigger list!", this);
                continue;
            }

            if (!trigger.IsDone)
            {
                allDone = false;
                break;
            }
        }

        if (allDone)
        {
            IsDone = true;
            OnActivated?.Invoke();
            //Debug.Log("CompositeTrigger activated!");
        }
    }

    // Для ручного добавления триггеров (например, из другого скрипта)
    public void AddTrigger(ActionsAreCommitted trigger)
    {
        if (trigger != null && !_requiredTriggers.Contains(trigger))
        {
            _requiredTriggers.Add(trigger);
        }
    }
}