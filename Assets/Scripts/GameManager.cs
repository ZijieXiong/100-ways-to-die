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
    }

    public float candleOffset = 5f;

    [SerializeField]
    private int alphaToGen = 16;

    private string directory = "/SaveData/";
    private string fileName = "library.txt";
    private GameObject[] candles;
    private Hashtable library;
    private Hashtable bank;
    private Word question;

    void Save(Hashtable library)
    {
        string dir = Application.streamingAssetsPath + directory;
        string res = "";
        if(!Directory.Exists(dir)){
            Directory.CreateDirectory(dir);
        }
        foreach(string key in library.Keys){
            //bool used = library[key];
            Word word = new Word(key, (int)library[key]);
            res += JsonUtility.ToJson(word) + '\n';
        }
        File.WriteAllText(dir + fileName, res);
    }


    void Load(Hashtable library)
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

    Hashtable GenerateBank(Hashtable library)
    {
        Hashtable bank = new Hashtable();
        foreach(string key in library.Keys){
            if((int)library[key] == 0){
                bank[key] = (int)library[key];
            }
        }
        return bank;
    }

    Word GenerateQuestion(Hashtable bank){
        int rand = Random.Range(0, bank.Count);
        int i = 0;
        Word word = new Word("");
        foreach(string key in bank.Keys){
            if(i == rand){
                word = new Word(key, (int)bank[key]);
                return word;
            }
            i = i + 1;
        }
        return word;
    }

    bool Answer(string key){
        if(library.ContainsKey(key)){
            library[key] = (int)library[key] + 1;
            return true;
        }
        else{
            return false;
        }

    }  

    private char[] GenerateRandomAlpha(Word q)
    {
        char[] alphas = new char[alphaToGen];
        string question = q.text;
        for(int i = question.Length;i<alphaToGen;i++){
            question += (char)('a'+Random.Range(0,26));
        }
        Debug.Log(question);
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
        return alphas;
    }

    // Start is called before the first frame update
    void Start()
    {   
        GameObject instance = Instantiate(Resources.Load("Alphabets/A", typeof(GameObject))) as GameObject;
        instance.GetComponent<Alphabet>().gm = this;
        candles = GameObject.FindGameObjectsWithTag("candle");
        library = new Hashtable();
        Load(library);
        bank = GenerateBank(library);
        question = GenerateQuestion(bank);
        Debug.Log(question.text);
        char[] alphas = GenerateRandomAlpha(question);
        //Answer("fire");
        Save(library);
        
    }

    public void dropAlphabet(Alphabet alpha)
    {
        foreach(GameObject candle in candles)
        {   
            if(alpha.boxcollider.bounds.Intersects(candle.GetComponent<BoxCollider2D>().bounds))
            {
                Debug.Log("Lock in " + candle.name);
                alpha.Lock();
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
