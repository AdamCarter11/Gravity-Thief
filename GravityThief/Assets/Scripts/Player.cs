using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Move Variables")]
    [SerializeField] float speed;
    private float moveVec;
    private float resetSpeed;
    private Rigidbody2D rb;
    private bool dir = true;

    //timer vars
    [Header("Timer Variables")]
    [SerializeField] TMP_Text timerText;
    private float totalTime = 0.0f;
    private bool startTimer = false;

    //jumping vars
    [Header("Jumping Variables")]
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] int amountOfJumps = 1;
    [SerializeField] Camera cam;
    private int resetJumps;
    private bool isGrounded;
    bool hitTheGround = true;

    //gravity vars
    [Header("Gravity Variables")]
    [SerializeField] float gravityTime;
    [SerializeField] Image gravityIndicator;
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
        //print(Mathf.Abs(rb.velocity.y));
        if(Input.anyKey){
            startTimer = true;
        }
        if(startTimer){
            totalTime += Time.deltaTime;
            timerText.text = Mathf.RoundToInt(totalTime).ToString();
        }

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
                gravityIndicator.enabled = true;
                gravityIndicator.fillAmount = gravityTime/2;
            }
            else{
                resetGravity();
                reverseGravity = false;
            }
        }
    }

    void JumpInputs(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if(!isGrounded){
            hitTheGround = false;
        }
        if(isGrounded){
            if(!hitTheGround){
                hitTheGround = true;
                if(Mathf.Abs(rb.velocity.y) > 10){
                    //cam.GetComponent<ScreenShake>().TriggerShake();
                    print(Mathf.Abs(rb.velocity.y));
                    print("test");
                    CinaShake.Instance.ShakeCamera(5f, .1f);
                }
            }
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
        if(!reverseGravity && isGrounded){
            gravityIndicator.enabled = false;
        }
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
