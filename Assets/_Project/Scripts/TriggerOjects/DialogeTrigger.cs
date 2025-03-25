using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class DialogeTrigger : MonoBehaviour, ITriggerable
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset _inkJSON;

    public void Trrigered()
    {
        DialogueManager dialogeManager = DialogueManager.GetInstance();
        dialogeManager.EnterDialogueMode(_inkJSON);
    }
}

