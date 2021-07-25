using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    public Conversation conversation;
    
    private int curInd;
    private int endInd;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartConversation(int startInd)
    {
        if(startInd >= 0 && startInd < conversation.dialogues.Length)
        {
            curInd = startInd;
            endInd = conversation.dialogues.Length;
            StartDialogue(conversation.dialogues[curInd]);
        }
    }

    public void StartConversation(int startInd, int lastInd)
    {   
        if(lastInd <= conversation.dialogues.Length && startInd < lastInd && startInd >= 0){
            curInd = startInd;
            endInd = lastInd;
            StartDialogue(conversation.dialogues[curInd]);
        }        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences = new Queue<string>();

        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence.words);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        curInd++;
        if(curInd < endInd)
        {
            StartDialogue(conversation.dialogues[curInd]);
        }
        else
        {
            animator.SetBool("IsOpen", false);
            //this.SetActive(false);
        }
    }
}
