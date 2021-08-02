using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{   
    public ConversationManager cm;
    public AudioSource storyBGM;

    void Awake()
    {
        storyBGM = GameObject.Find("StoryBGM").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cm.StartConversation(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(cm.isEnd)
        {   
            storyBGM.Stop();
            SceneManager.LoadScene("StartMenu");
        }
    }
}
