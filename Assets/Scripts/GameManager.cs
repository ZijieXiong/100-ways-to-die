using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static DatabaseManager;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public bool debug = false;
    public GameObject[] lights;

    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private float initialAlphaX = 0f;
    [SerializeField]
    private float intiialAlphaY = 1.85f;
    [SerializeField]
    private float alphaOffsetX = 0.3f;
    [SerializeField]
    private float alphaOffsetY = 0.3f;
    [SerializeField]
    private float candleOffset = 5f;
    [SerializeField]
    private int alphaLine = 2;
    [SerializeField]
    private int alphaToGen = 8;
    [SerializeField]
    private float alphaBrightnessOnClick = 5f;
    [SerializeField]
    private float alphaBrightnessOffClick = 0.55f;
    private string fileName = "";
    private GameObject[] candles;
    private GameObject[] letters;
    private GameObject op;
    private GameObject ed;
    private Hashtable library;
    private Hashtable stages;
    private Word curQuestion;
    private Character curStage;
    private char[] alphas;
    private GameObject tool;
    private ConversationManager opcm;
    private ConversationManager edcm;


    
    public void Combine()
    {
        if(CheckSpell())
        {   
            if(!debug)
            {
                Answer();
            }
            TurnLight(false);
            //tool = Instantiate(Resources.Load("Tools/" + curQuestion.text, typeof(GameObject))) as GameObject;
            edcm.StartConversation(0);
        }
        else
        {
            Reset();
        }
    }

    public void Cancel()
    {
        Reset();
    }
    
    public void Story()
    {
        opcm.StartConversation(0);
    }  

    //handle drop event for letters
    public void dropAlphabet(Alphabet alpha)
    {
        foreach(GameObject candle in candles)
        {   
            Candle candleScript = candle.GetComponent<Candle>();
            if(alpha.boxcollider.bounds.Intersects(candle.GetComponent<BoxCollider2D>().bounds) && !candleScript.isLock)
            {
                candleScript.letter = alpha.GetLetter();
                candleScript.isLock = true;
                GameObject child = candle.transform.GetChild(0).gameObject;
                child.SetActive(true);
                alpha.Lock();
                alpha.gameObject.layer = 2;
                alpha.SetFire(false);
                alpha.transform.position = new Vector3(candle.transform.position.x, candle.transform.position.y + candleOffset, candle.transform.position.z);
                break;
            }
        }
    }

    public void TurnLight(bool isOn)
    {
        foreach(GameObject light in lights)
        {
            light.SetActive(isOn);
        }
    }

    //randomly generate a stage that has question left to answer
    void GenerateStage()
    {
        Hashtable notCleared = new Hashtable();
        foreach(string key in stages.Keys)
        {
            if((int)stages[key]!=0){
                notCleared[key] = (int)stages[key];
            }
        }
        int rand = Random.Range(0, notCleared.Count);
        int i = 0;
        foreach(string key in notCleared.Keys)
        {
            if(i == rand)
            {   
                curStage = new Character(key, (int)notCleared[key]);
            }
        }
        fileName = curStage.name + ".txt";
    }

    //randomly generate a question from current library
    void GenerateQuestion(){
        Hashtable bank = new Hashtable();
        foreach(string key in library.Keys){
            if((int)library[key] == 0){
                bank[key] = (int)library[key];
            }
        }
        int rand = Random.Range(0, bank.Count);
        int i = 0;
        foreach(string key in bank.Keys){
            if(i == rand){
                curQuestion = new Word(key, (int)bank[key]);
            }
            i = i + 1;
        }
    }

    //Record if correctly anwered the question
    void Answer(){

        library[curQuestion.text] = (int)library[curQuestion.text] + 1;
        stages[curStage.name] = (int)stages[curStage.name] - 1;
    }  

    //Generate a array of random letters that contains current question
    private void GenerateRandomAlpha()
    {
        alphas = new char[alphaToGen];
        string question = curQuestion.text;
        for(int i = question.Length;i<alphaToGen;i++){
            question += (char)('a'+Random.Range(0,26));
        }
        for(int i = 0;i < alphas.Length;i++)
        {
            alphas[i] = ' ';
        }
        for(int i = 0; i < alphas.Length;i++)
        {
            int r = Random.Range(0, alphaToGen);
            while(alphas[r] != ' ')
            {
                r = Random.Range(0, alphaToGen);
            }
            alphas[r] = question[i];
        }
    }

    private void SetAlphaBrightness(GameObject[] letters)
    {
        for(int i = 0; i < 10; i++)
        {
            letters[i].GetComponent<Alphabet>().BrightnessOnClick = alphaBrightnessOnClick;
            letters[i].GetComponent<Alphabet>().BrightnessOffClick = alphaBrightnessOffClick;
        }
    }

    //Load alphabet prefab
    private void LoadAlpha()
    {   
        letters = new GameObject[alphaToGen];
        for(int i = 0;  i < alphas.Length; i++)
        {
            letters[i] = Instantiate(Resources.Load("Alphabets/" + alphas[i].ToString(), typeof(GameObject))) as GameObject;
            Alphabet letterScript = letters[i].GetComponent<Alphabet>();
            letterScript.gm = this;
            
        }
        SetAlphaBrightness(letters);
        SetAlphaPos();
    }

    //Set up intial position for alphas
    private void SetAlphaPos()
    {   
        int counter = 0;
        float x = initialAlphaX;
        float y = intiialAlphaY;
        for(int i = 0; i < letters.Length; i++)
        {
            letters[i].transform.position = new Vector3(x, y, letters[i].transform.position.z);
            x += alphaOffsetX;
            counter += 1;
            if(counter >= letters.Length/alphaLine)
            {
                counter = 0;
                x = initialAlphaX;
                y += alphaOffsetY;
            }

        }
    }

    private void LoadOp()
    {
        op = Instantiate(Resources.Load("UI/" + "Opening" + curStage.name, typeof(GameObject))) as GameObject;
        op.transform.SetParent(canvas.transform, false);
        opcm = op.GetComponent<ConversationManager>();
    }

    private void LoadED()
    {
        ed = Instantiate(Resources.Load("UI/" + "Ending" + curStage.name, typeof(GameObject))) as GameObject;
        ed.transform.SetParent(canvas.transform, false);
        edcm = ed.GetComponent<ConversationManager>();
    }

    private void Save()
    {
        DatabaseManager.SaveLibrary(library, fileName);
        DatabaseManager.SaveStage(stages);
    }

    //Check if current alphas position spell the correct answer
    private bool CheckSpell()
    {   
        string answer = "";
        foreach(GameObject candle in candles)
        {
            answer += candle.GetComponent<Candle>().letter; 
        }
        return (answer == curQuestion.text);
    } 

    private void Reset()
    {
        SetAlphaPos();
        foreach(GameObject candle in candles)
        {   
            Candle candleScript = candle.GetComponent<Candle>();
            candleScript.letter = "";
            candleScript.isLock = false;
            GameObject child = candle.transform.GetChild(0).gameObject;
            child.SetActive(false);
        }
        foreach(GameObject letter in letters)
        {
            letter.layer = 0;
            Alphabet alpha = letter.GetComponent<Alphabet>();
            alpha.Unlock();
            alpha.SetFire(true);            
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        candles = GameObject.FindGameObjectsWithTag("candle");
        library = new Hashtable();
        stages = new Hashtable();
        DatabaseManager.LoadStage(stages);
        GenerateStage();
        DatabaseManager.LoadLibrary(library, fileName);
        GenerateQuestion();
        GenerateRandomAlpha();
        LoadAlpha();
        LoadOp();
        LoadED();
        opcm.StartConversation(0);
        TurnLight(false);
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(opcm.isEnd)
        {
            TurnLight(true);
        }
        if(edcm.isEnd)
        {   
            Save();
            SceneManager.LoadScene("PlayScene");
        }
    }
}
