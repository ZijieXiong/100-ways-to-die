using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Word
    {
        public string text;
        [SerializeField]
        int used = 0;

        public Word(string t){
            text = t;
        }

        public Word(string t, int u){
            text = t;
            used = u;
        }

        public int ifUsed(){
            return used;
        }

        public void Answer()
        {
            used += 1;
        }
    }

    [System.Serializable]
    public class Character
    {
        public string name;

        public int questionLeft;

        public Character(string n, int q)
        {
            name = n;
            questionLeft = q;
        }

    }
    [SerializeField]
    private float candleOffset = 5f;
    [SerializeField]
    private int alphaToGen = 8;
    private string directory = "/SaveData/";
    private string stageFile = "Stages.txt";
    private string fileName = "";
    private GameObject[] candles;
    private GameObject[] letters;
    private Hashtable library;
    private Hashtable stages;
    private Word curQuestion;
    private Character curStage;
    private char[] alphas;

    //Save stage and current library into local files
    void Save()
    {
        string dir = Application.streamingAssetsPath + directory;
        string res = "";
        if(!Directory.Exists(dir)){
            Directory.CreateDirectory(dir);
        }
        foreach(string key in library.Keys){
            Word word = new Word(key, (int)library[key]);
            res += JsonUtility.ToJson(word) + '\n';
        }
        File.WriteAllText(dir + fileName, res);
        res = "";
        foreach(string key in stages.Keys){
            Character chara = new Character(key, (int)stages[key]);
            res += JsonUtility.ToJson(chara) + '\n';
        }
        File.WriteAllText(dir + stageFile, res);
    }

    //load stage from local file
    void loadStage()
    {
        string fullPath = Application.streamingAssetsPath + directory + stageFile;

        if(File.Exists(fullPath))
        {
            foreach (string line in File.ReadLines(fullPath))
            {
                Character chara = JsonUtility.FromJson<Character>(line);
                stages[chara.name] = chara.questionLeft;
            }
        }
    }

    //load library from local file
    void LoadLibrary()
    {
        string fullPath = Application.streamingAssetsPath + directory + fileName;
        
        if(File.Exists(fullPath))
        {
            foreach (string line in File.ReadLines(fullPath)){
                Word word = JsonUtility.FromJson<Word>(line);
                library[word.text] = word.ifUsed();
            }
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
        stages[curStage.name] = (int)library[curStage.name] - 1;
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

    //Load alphabet prefab
    private void LoadAlpha()
    {   
        letters = new GameObject[alphaToGen];
        for(int i = 0;  i < alphas.Length; i++)
        {
            letters[i] = Instantiate(Resources.Load("Alphabets/" + alphas[i].ToString(), typeof(GameObject))) as GameObject;
            letters[i].GetComponent<Alphabet>().gm = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        //GameObject instance = Instantiate(Resources.Load("Alphabets/A", typeof(GameObject))) as GameObject;
        //instance.GetComponent<Alphabet>().gm = this;
        candles = GameObject.FindGameObjectsWithTag("candle");
        library = new Hashtable();
        stages = new Hashtable();
        loadStage();
        GenerateStage();
        LoadLibrary();
        GenerateQuestion();
        GenerateRandomAlpha();
        //Answer();
        LoadAlpha();
        Save();
        
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
                Debug.Log(candleScript.letter);
                candleScript.isLock = true;
                alpha.Lock();
                alpha.gameObject.layer = 2;
                GameObject child = candle.transform.GetChild(0).gameObject;
                child.SetActive(true);
                alpha.EliminateFire();
                alpha.transform.position = new Vector3(candle.transform.position.x, candle.transform.position.y + candleOffset, candle.transform.position.z);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
