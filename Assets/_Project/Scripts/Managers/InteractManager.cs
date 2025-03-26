using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private ITriggerable currentTriggerable;
    private GameObject interactionIndicator;
    private bool hasInteracted = false; // Флаг, указывающий на то, что взаимодействие уже произошло
    private Collider currentTriggerCollider; // Запоминаем текущий коллайдер триггера

    private void OnTriggerEnter(Collider collider)
    {
        // Если это новый триггер или тот же, но мы выходили и зашли снова
        if (currentTriggerCollider != collider || !hasInteracted)
        {
            currentTriggerable = collider.gameObject.GetComponent<ITriggerable>();
            currentTriggerCollider = collider;
            hasInteracted = false; // Сбрасываем флаг при входе в новый триггер

            if (currentTriggerable != null)
            {
                var indicators = collider.gameObject.GetComponentsInChildren<Transform>(true);
                foreach (var indicator in indicators)
                {
                    if (indicator.CompareTag("InteractionIndicator"))
                    {
                        interactionIndicator = indicator.gameObject;
                        interactionIndicator.SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider == currentTriggerCollider)
        {
            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false);
            }

            currentTriggerable = null;
            currentTriggerCollider = null;
            interactionIndicator = null;
            hasInteracted = false; // Сбрасываем флаг при выходе из триггера
        }
    }

    private void Update()
    {
        if (currentTriggerable != null && !hasInteracted)
        {
            bool isInteract = InputManager.GetInstance().GetInteractPressed();

            if (isInteract)
            {
                Debug.Log("Interacting with: " + currentTriggerable);
                currentTriggerable.Trrigered();
                hasInteracted = true; // Устанавливаем флаг после взаимодействия

                if (interactionIndicator != null)
                {
                    interactionIndicator.SetActive(false);
                }
            }
        }
    }
}