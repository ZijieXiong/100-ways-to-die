using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationManager : MonoBehaviour
{   
    public GameManager gm;
    public Text nameText;
    public Text dialogueText;
    public bool isEnd = false;
    public Animator[] animators;
    public Conversation conversation;   
    private Queue<Sentence> sentences;
    private bool isStart = true;

    public AudioSource button;

    
    
    private int curInd;
    private int endInd;

    // Start is called before the first frame update
    void Start()
    {
        isEnd = false;
    }

    public void SetIsStart(bool val)
    {
        isStart = val;
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
        foreach(Animator animator in animators)
        {
            animator.SetBool("IsOpen", true);
        } 

        nameText.text = dialogue.name;

        sentences = new Queue<Sentence>();
        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        isEnd = false;
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {   
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        if(!isStart)
        {
            button.Play();
        }
        else
        {
            isStart = false;
        }

        Sentence sentence = sentences.Dequeue();

        ChangeAnimation(sentence.animationChanges);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence.words));
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
            foreach(Animator animator in animators)
            {
                animator.SetBool("IsOpen", false);
            }
            if(gm!=null)
            {
                gm.TurnLight(true);
                gm.StopStoryBGM(this);
            }
            isEnd = true;
        }
    }

    void ChangeAnimation(AnimationChange[] animationChanges)
    {   
        foreach(AnimationChange ac in animationChanges)
        {   
            if(ac != null)
            {
                GameObject character = transform.Find(ac.charName).gameObject;
                Animator animator = character.GetComponent<Animator>();
                animator.SetBool(ac.varName, ac.varValue);
            }
           

        }
    }

    void Update()
    {
    }
}
