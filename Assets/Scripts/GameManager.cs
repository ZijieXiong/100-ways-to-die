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

    string directory = "/SaveData/";
    string fileName = "library.txt";
    Hashtable library;
    Hashtable bank;
    Word question;

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


    // Start is called before the first frame update
    void Start()
    {
        library = new Hashtable();
        Load(library);
        bank = GenerateBank(library);
        question = GenerateQuestion(bank);
        Debug.Log(question.text);
        //Answer("fire");
        Save(library);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
