using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

static public class DatabaseManager
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

    static private string directory = "/SaveData/";
    static private string stageFile = "Stages.txt";

    static public void ResetDatabase()
    {
        Hashtable stages = new Hashtable();
        LoadStage(stages);
        List<string> keys = new List<string>();
        foreach(string key in stages.Keys)
        {
            keys.Add(key);
        }
        foreach(string key in keys)
        {
            Hashtable library = new Hashtable();
            LoadLibrary(library, key + ".txt");
            ResetLibrary(library);
            SaveLibrary(library, key + ".txt");
            stages[key] = library.Count;
        }
        SaveStage(stages);
    }

    static public void ResetLibrary(Hashtable library)
    {   
        List<string> keys = new List<string>();
        foreach(string key in library.Keys)
        {
            keys.Add(key);
        }    
        foreach(string key in keys)
        {
            library[key] = 0;
        }
    }

    static public int QuestionLeft(Hashtable stages)
    {   
        int numOfQuestion = 0;
        foreach(string key in stages.Keys)
        {
            numOfQuestion += (int)stages[key];
        }
        return numOfQuestion;
    }

    static public void LoadStage(Hashtable stages)
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

    static public void LoadLibrary(Hashtable library, string fileName)
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

        //Save stage and current library into local files
    static public void SaveStage(Hashtable stages)
    {
        string dir = Application.streamingAssetsPath + directory;
        string res = "";
        if(!Directory.Exists(dir)){
            Directory.CreateDirectory(dir);
        }
        foreach(string key in stages.Keys){
            Character chara = new Character(key, (int)stages[key]);
            res += JsonUtility.ToJson(chara) + '\n';
        }
        File.WriteAllText(dir + stageFile, res);
    }

    static public void SaveLibrary(Hashtable library, string fileName)
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
    }
}
