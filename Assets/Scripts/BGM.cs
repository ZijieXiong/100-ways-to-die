using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{   
    static BGM s;

    void Awake(){
        if(s == null){
            s = this;
        }
        else if(s != this){
            Destroy(gameObject);
            Debug.Log(gameObject.name);
            return;
        }
        Debug.Log("Dont destroy" + s.gameObject.name);
        DontDestroyOnLoad(s.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
