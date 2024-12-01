using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour
{

    private ITriggerable currentTriggerable; 

    private void OnTriggerEnter(Collider collider)
    {
        currentTriggerable = null;
        currentTriggerable = collider.gameObject.GetComponent<ITriggerable>();

        if (currentTriggerable != null)
        {
            //Debug.Log("����� ����� ����������������� � ��������.");
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<ITriggerable>() == currentTriggerable)
        {
            currentTriggerable = null;
            //Debug.Log("����� ������� ���� ��������������.");
        }
    }

    private void Update()
    {        
        if (currentTriggerable != null)
        {
            Debug.Log("����� ��������������� � ��������.");
            currentTriggerable.Trrigered();
        }
    }
}
