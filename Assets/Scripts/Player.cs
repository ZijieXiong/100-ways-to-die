using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public float moveSpeed = 10f;
    [Range(0,10)] public float jumpVelocity = 5f;
    public LayerMask mask;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float squadMultiplier = 0.7f;

    private float standHeight;
    private float squadHeight;
    private float changeOfHeight;
    private Vector2 playerSize;
    private Vector2 boxSize;
    private Animator animator;
    private Rigidbody2D _rigidbody2D;
    private bool jumpRequest=false;
    private bool grounded = false;
    private bool isRoll = false;
    private BoxCollider2D playerCollider;
    private Vector3 moveX;
    Vector2 boxCenter;
    
    
    private void Squad()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, squadHeight);
        moveSpeed = 0.5f * moveSpeed;
    }

    private void Stand()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, standHeight);
        moveSpeed = 2f * moveSpeed;
    }

    private void Roll()
    {
        animator.SetBool("Roll", false);
        playerCollider.size = new Vector2(playerCollider.size.x, standHeight);
        isRoll = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        playerCollider = GetComponent<BoxCollider2D>();
        boxSize = new Vector2(playerSize.x*0.1f, playerSize.y*0.05f);
        mask = LayerMask.GetMask("Wall");
        standHeight = playerCollider.size.y;
        squadHeight = standHeight * squadMultiplier;
        changeOfHeight = standHeight - squadHeight;
        animator = GetComponent<Animator>();
        moveX = new Vector3();

    }

    // Update is called once per frame
    void Update()
    {   
        if(!isRoll){
            moveX = new Vector3();  
        }
        if(Input.GetButtonDown("Jump") && grounded){
            jumpRequest=true;
        }
        if(Input.GetKey(KeyCode.D)){
            moveX = Vector3.right;
            animator.SetInteger("Walk", 1);
        }
        else if(Input.GetKey(KeyCode.A)){
            moveX = Vector3.left;
            animator.SetInteger("Walk", -1);
        }
        else{
            animator.SetInteger("Walk", 0);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isRoll){
            animator.SetBool("Roll", true);
            if(moveX.x == 0){
                moveX.x = 1;
            }
            isRoll = true;
            playerCollider.size = new Vector2(playerCollider.size.x, squadHeight);
            Invoke("Roll", 0.75f);
        }

        if(Input.GetKeyDown(KeyCode.LeftControl)){
            Squad();
            animator.SetBool("Squad", true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl)){
            Stand();
            animator.SetBool("Squad", false);
        }
        transform.Translate(moveX*moveSpeed*Time.deltaTime);
    }

    private void FixedUpdate(){
        if(jumpRequest)
        {
            _rigidbody2D.AddForce(Vector3.up*jumpVelocity, ForceMode2D.Impulse);
            jumpRequest=false;
            grounded=false;
            if(!animator.GetBool("Jump")){
                animator.SetBool("Jump", true);
            }
        }
        else{
            boxCenter = (Vector2)transform.position + (Vector2.down * playerCollider.size.y*7.0f); 
            if(Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null){
                if(!grounded && _rigidbody2D.velocity.y < 0)
                {  
                    animator.SetBool("Jump", false);
                }
                grounded=true;
            }
            else{
                grounded=false;
            }
        }
        _rigidbody2D.gravityScale = fallMultiplier;   
        if(Input.GetButton("Jump")){
            _rigidbody2D.gravityScale = lowJumpMultiplier;
        }
    }


    private void OnDrawGizmosSelected(){
        if(grounded){
            Gizmos.color = Color.red;
        }
        else{
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireCube(boxCenter,boxSize);
    }

}
