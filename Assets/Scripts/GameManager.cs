using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Word
    {
        public string text;
        [SerializeField]
        bool used = false;

        public Word(string t){
            text = t;
        }
    }

    public class Library
    {
        public ArrayList words;

        public Library(){
            words = new ArrayList();
        }
        public Library(ArrayList newWords){
            foreach (Word word in newWords)
            {
                words.Add(word);
            }
        }
    }

    string directory = "/SaveData/";
    string fileName = "library.txt";

    void Save(string json)
    {
        string dir = Application.streamingAssetsPath + directory;

        if(!Directory.Exists(dir)){
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(dir + fileName, json);
    }

    // Start is called before the first frame update
    void Start()
    {
        Library library = new Library();
        library.words.Add(new Word("fire"));
        string jsonString = JsonUtility.ToJson(new Word("fire"));
        Save(jsonString);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
