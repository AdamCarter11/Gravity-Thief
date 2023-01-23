using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField] TMP_Text moneyText;
    private int money;
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
        money = 0;

        if(!PlayerPrefs.HasKey("Time")){
            PlayerPrefs.SetInt("Time", 0);
            PlayerPrefs.SetInt("Money", 0);
        }
    }

    void Update()
    {
        UIFuncs();

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

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("GravPickUp")){
            gravityTime = resetGravTime;
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("HiddenWall")){
            Color tempAlpha = other.GetComponent<SpriteRenderer>().color;
            tempAlpha.a = .5f;
            other.GetComponent<SpriteRenderer>().color = tempAlpha;
        }

        if(other.gameObject.CompareTag("Money")){
            money++;
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("End")){
            if(PlayerPrefs.GetInt("Time") == 0 || money > PlayerPrefs.GetInt("Money") || totalTime < PlayerPrefs.GetInt("Time") && money >= PlayerPrefs.GetInt("Money")){
                PlayerPrefs.SetInt("Money", money);
                PlayerPrefs.SetInt("Time", (int)totalTime);
            }
            SceneManager.LoadScene("EndScene");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("HiddenWall")){
            Color tempAlpha = other.GetComponent<SpriteRenderer>().color;
            tempAlpha.a = 255;
            other.GetComponent<SpriteRenderer>().color = tempAlpha;
        }
    }

    void UIFuncs(){
        if(Input.anyKey){
            startTimer = true;
        }
        if(startTimer){
            totalTime += Time.deltaTime;
            timerText.text = "Time: " + (Mathf.RoundToInt(totalTime).ToString());
        }
        moneyText.text = "Money: " + money;
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
                if(Mathf.Abs(rb.velocity.y) > 5){
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
