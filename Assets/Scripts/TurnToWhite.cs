using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToWhite : MonoBehaviour
{
    public GameObject img1;
    public GameObject img2;
    public GameObject img3;
    public float timeFrom0To1;
    public float timeFrom1To2;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("TurnToWhite", true);
            Invoke("ChangeImage", timeFrom0To1);
        }
    }

    void ChangeImage()
    {
        img1.SetActive(true);
        Invoke("DisplayNextImage", timeFrom1To2);
    }

    void DisplayNextImage()
    {
        img2.SetActive(true);
    }

}
