using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DatabaseManager;

public class StartMenuManager : MonoBehaviour
{
    public GameObject prologue;
    public void NewGame()
    {
        DatabaseManager.ResetDatabase();
        GameStart();
    }

    public void Continue()
    {
        GameStart();
    }

    private void GameStart()
    {}

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
