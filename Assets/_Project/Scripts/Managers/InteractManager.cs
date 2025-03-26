using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private ITriggerable currentTriggerable;
    private GameObject interactionIndicator;
    private bool hasInteracted = false; // ����, ����������� �� ��, ��� �������������� ��� ���������
    private Collider currentTriggerCollider; // ���������� ������� ��������� ��������

    private void OnTriggerEnter(Collider collider)
    {
        // ���� ��� ����� ������� ��� ��� ��, �� �� �������� � ����� �����
        if (currentTriggerCollider != collider || !hasInteracted)
        {
            currentTriggerable = collider.gameObject.GetComponent<ITriggerable>();
            currentTriggerCollider = collider;
            hasInteracted = false; // ���������� ���� ��� ����� � ����� �������

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
            hasInteracted = false; // ���������� ���� ��� ������ �� ��������
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
                hasInteracted = true; // ������������� ���� ����� ��������������

                if (interactionIndicator != null)
                {
                    interactionIndicator.SetActive(false);
                }
            }
        }
    }
}