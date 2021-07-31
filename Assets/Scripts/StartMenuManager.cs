using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DatabaseManager;

public class StartMenuManager : MonoBehaviour
{
    public GameObject prologue;
    public ConversationManager cm;
    public void NewGame()
    {
        DatabaseManager.ResetDatabase();
        cm.StartConversation(0);
    }

    public void Continue()
    {
        GameStart();
    }

    private void GameStart()
    {
        SceneManager.LoadScene("PlayScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if(cm.isEnd)
        {
            GameStart();
        }
    }
}
