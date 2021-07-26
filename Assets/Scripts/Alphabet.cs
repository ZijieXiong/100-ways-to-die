using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static System.Math;

public class Alphabet : MonoBehaviour
{
    public GameManager gm;
    public BoxCollider2D boxcollider;
    private Vector3 screenPoint;
    //private Vector3 offset;
    private float dragOffset = 0.3f;
    private bool isLocked = false;
    [SerializeField]
    private string letter;
    private GameObject fire;
    Renderer rend;
    private float timer = 0f;

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void SetFire(bool isAct)
    {
        GameObject fire = transform.GetChild(0).gameObject;
        fire.SetActive(isAct);
    }

    public string GetLetter()
    {
        return letter;
    }

    void Start()
    {
        fire = transform.GetChild(0).gameObject;
        rend  =fire.GetComponent<Renderer>();
    }

    private void SetFireBrightness(float brightness)
    {
        GameObject light2D = fire.transform.GetChild(0).gameObject;
        Debug.Log(light2D.name);
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        SetFireBrightness(10f);
    }


    void OnMouseDrag()
    {   
        if(!isLocked)
        {
            Vector3 curPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            curPos.z = 0f;
            float speed = 300 * Abs(transform.position.x - curPos.x);
            if(speed>dragOffset)
            {
                if(transform.position.x < curPos.x)
                {
                    rend.material.SetColor("distortion_speed", new Color(speed, -0.51f, 0f, 0f));
                }
                else if(transform.position.x > curPos.x)
                {
                    rend.material.SetColor("distortion_speed", new Color(-speed, -0.51f, 0f, 0f));
                }
                timer = 0f;
            }
            else if(speed == 0)
            {   
                timer += Time.deltaTime;
                if(timer > 0.15f)
                {
                    rend.material.SetColor("distortion_speed", new Color(-0.07f, -0.51f, 0f, 0f));
                }
            }
            transform.position = curPos;
            
        }
        
    }

    void OnMouseUp()
    {   
        rend.material.SetColor("distortion_speed", new Color(-0.07f, -0.51f, 0f, 0f));
        if(!isLocked)
        {
            gm.dropAlphabet(this);
        }
    }



}
