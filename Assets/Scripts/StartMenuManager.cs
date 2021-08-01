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
        Invoke("GameStart", 1);
    }

    public void Quit()
    {        
        Application.Quit();
    }

    private void GameStart()
    {
        SceneManager.LoadScene("PlayScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void StartStory()
    {
        cm.StartConversation(0);
        storyBGM.Play();
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
