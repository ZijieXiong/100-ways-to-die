using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{   
    public ConversationManager cm;
    public AudioSource storyBGM;
    public Animator fade;

    void ReturnMain()
    {
        SceneManager.LoadScene("StartMenu");
        storyBGM.Stop();
    }

    void Awake()
    {
        storyBGM = GameObject.Find("StoryBGM").GetComponent<AudioSource>();
        if(!storyBGM.isPlaying)
        {
            storyBGM.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fade.SetBool("IsMasked", false);
        cm.StartConversation(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(cm.isEnd)
        {   
            fade.SetBool("IsMasked", true);
            Invoke("ReturnMain", 1);
        }
    }
}
