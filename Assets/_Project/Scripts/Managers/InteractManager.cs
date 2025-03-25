using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private ITriggerable currentTriggerable;
    private GameObject interactionIndicator; // Добавляем переменную для хранения индикатора

    private void OnTriggerEnter(Collider collider)
    {
        currentTriggerable = null;
        currentTriggerable = collider.gameObject.GetComponent<ITriggerable>();

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

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<ITriggerable>() == currentTriggerable)
        {
            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false); 
            }

            currentTriggerable = null;
            interactionIndicator = null; 
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        currentTriggerable = collider.gameObject.GetComponent<ITriggerable>();

        if (currentTriggerable != null)
        {
            //Debug.Log("Игрок находится внутри триггера и может взаимодействовать.");
        }
    }

    private void Update()
    {
        bool isInteract = InputManager.GetInstance().GetInteractPressed();
        if (isInteract)
        {
            Debug.Log("isInteract " + isInteract);
        }
        if (currentTriggerable != null)
        {
            Debug.Log("currentTriggerable " + currentTriggerable);
        }
        if (currentTriggerable != null && isInteract)
        {
            //Debug.Log("Игрок взаимодействует с объектом.");
            currentTriggerable.Trrigered();

            if (interactionIndicator != null)
            {
                interactionIndicator.SetActive(false); 
            }
        }
    }
}