using UnityEngine;
using System;

public class Lever : MonoBehaviour, ICheckableTrigger
{
    public event Action OnActivated;
    public bool IsDone { get; private set; }
    public void Trrigered()
    {
        IsDone = true;
        //Debug.Log("����� �����������!");
        // ������ ��������� ������

        OnActivated?.Invoke();
    }
}
