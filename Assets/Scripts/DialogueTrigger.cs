using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Conversation conversation;

    public void TriggerDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(conversation.dialogues[0]);
    }

}
