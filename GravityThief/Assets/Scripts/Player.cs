using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    private float moveVec;
    private float resetSpeed;
    private Rigidbody2D rb;
    private bool dir = true;

    //jumping vars
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] int amountOfJumps = 1;
    private int resetJumps;
    private bool isGrounded;

    //gravity vars
    [SerializeField] float gravityTime;
    float resetGravTime;
    bool reverseGravity = false;
    bool gravityTimeIsRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        resetJumps = amountOfJumps;
        resetSpeed = speed;
        resetGravTime = gravityTime;
    }

    void Update()
    {
        moveVec = Input.GetAxis("Horizontal");
        if(dir == false && moveVec > 0){
            Flip();
        }
        if(dir && moveVec < 0){
            Flip();
        }

        TimerFunc();
        
        JumpInputs();

        GravityFunc();
        
    }

    void FixedUpdate() 
    {
        rb.velocity = new Vector2(moveVec * speed, rb.velocity.y);
    }

    void Flip(){
        dir = !dir;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void TimerFunc(){
        if(gravityTimeIsRunning){
            if(gravityTime > 0){
                gravityTime -= Time.deltaTime;
            }
            else{
                resetGravity();
                reverseGravity = false;
            }
        }
    }

    void JumpInputs(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if(isGrounded){
            amountOfJumps = resetJumps;
            speed = resetSpeed;

            if(!reverseGravity){
                gravityTime = resetGravTime;
            }
        }
        else if(speed >= 10){
            speed /= 2;
        }
        if(Input.GetKeyDown(KeyCode.Space) && amountOfJumps > 0){
            rb.velocity = Vector2.up * jumpForce;
            amountOfJumps--;
        }
    }

    void GravityFunc(){
        if(Input.GetMouseButtonDown(0) && !reverseGravity && gravityTime > 0){
            gravityTimeIsRunning = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            reverseGravity = true;
            Vector3 scaler = transform.localScale;
            scaler.y *= -1;
            transform.localScale = scaler;
            rb.gravityScale = -1;
        }
        else if(Input.GetMouseButtonDown(0) && reverseGravity){
            resetGravity();
        }
    }
    void resetGravity(){
        gravityTimeIsRunning = false;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        reverseGravity = false;
        Vector3 scaler = transform.localScale;
        scaler.y = Mathf.Abs(scaler.y);
        transform.localScale = scaler;
        rb.gravityScale = 1;
    }
}
