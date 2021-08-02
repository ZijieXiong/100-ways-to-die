using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DatabaseManager;

public class StartMenuManager : MonoBehaviour
{
    public GameObject prologue;
    public ConversationManager cm;
    public AudioSource bgm;
    public AudioSource newGame;
    public AudioSource continueButton;
    public AudioSource storyBGM;
    public GameObject buttons;

    public void NewGame()
    {
        DatabaseManager.ResetDatabase();
        newGame.Play();
        bgm.Stop();
        Invoke("StartStory", 1);
    }

    public void Continue()
    {   
        continueButton.Play();
        bgm.Stop();
        Invoke("GameStart", 1);
        storyBGM.PlayDelayed(1);
    }

    public void Quit()
    {        
        Application.Quit();
    }

    void GameStart()
    {
        SceneManager.LoadScene("PlayScene");
    }

    void Awake()
    {
        storyBGM = GameObject.Find("StoryBGM").GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void StartStory()
    {
        cm.StartConversation(0);
        storyBGM.Play();
        buttons.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(cm.isEnd)
        {
            Invoke("GameStart", 1);
        }
    }
}
