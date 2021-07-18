using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Alphabet : MonoBehaviour
{
    public GameManager gm;
    public BoxCollider2D boxcollider;
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isLocked = false;
    [SerializeField]
    private string letter;
    public void Lock()
    {
        isLocked = true;
    }

    public void EliminateFire()
    {
        GameObject fire = transform.GetChild(0).gameObject;
        fire.SetActive(false);
    }

    public string GetLetter()
    {
        return letter;
    }
    void Start()
    {
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

    }


    void OnMouseDrag()
    {   
        if(!isLocked)
        {
            Vector3 curPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            curPos.z = 0f;
            transform.position = curPos;
        }
        
    }

    void OnMouseUp()
    {   
        if(!isLocked)
        {
            gm.dropAlphabet(this);
        }
    }



}
