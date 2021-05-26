using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerCamera;
    private Animator anim;
    private Rigidbody rigidbody;
    PlayerStats playerStats;
    PlayerAnimation playerAnimation;
    PlayerAction playerAction;

    //Animator Values
    private int _zVelHash;
    private int _xVelHash;
    private float _zVel = 0f;
    private float _xVel = 0f;

    //Move Setting
    public float turnSmooth = 0.1f;
    float turnSmoothVelocity;
    public float horizontalVelocity;
    public bool _sprinting;
    public bool moveKeyPressed = false;

    //Jump Setting
    public float verticalVelocity;

    private Transform CameraPivot; //empty point on player
    public CharacterController myController;

    private Vector3 camDirection;
    public bool isOnKnockBack = false;

    //Sprint
    public bool isDodging = false;
    public float DodgeTime = 0f;
    public bool isSprinting = false;
    public float consumeStaminaSpeedTime = 0;

    //Remove later once network implementation is finished
    public bool player2;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //this.CameraPivot = this.playerCamera.transform.parent;
        myController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        _zVelHash = Animator.StringToHash("velZ");
        _xVelHash = Animator.StringToHash("velX");

        playerStats = GetComponent<PlayerStats>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAction = GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if (!playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("BI") && !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PB")
            && !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT") && !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT") && !playerStats.isHitStun)
        {
            Movement(forwardPressed, rightPressed, leftPressed, backPressed, runPressed);
            if(forwardPressed || rightPressed || leftPressed || backPressed)
            {
                moveKeyPressed = true;
            }
            else if(!forwardPressed || !rightPressed || !leftPressed ||! backPressed)
            {
                moveKeyPressed = false;
            }
        }

        anim.SetFloat(_xVelHash, _xVel);
        anim.SetFloat(_zVelHash, _zVel);
    }

    private void Sprint()
    {
        if(isSprinting && !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("A"))
        { 
            if (playerStats.stamina > 0)
            {
                playerStats.speed = 8f;
                playerStats.readyToRestoreStaminaTime = playerStats.setReadyToRestoreStaminaTime(3.0f);

                if (consumeStaminaSpeedTime <= 0)
                {
                    playerStats.stamina -= 2;
                    consumeStaminaSpeedTime = setConsumeStaminaTime();
                }
                if (consumeStaminaSpeedTime > 0 && GameObject.Find("Player").transform.hasChanged == true)
                {
                    consumeStaminaSpeedTime -= Time.fixedDeltaTime;
                }
            }
        }

        if ((!isSprinting || playerStats.stamina == 0) && !playerAction.isKeepBlocking)
        {
            playerStats.speed = 4f;
            consumeStaminaSpeedTime = setConsumeStaminaTime();
        }
    }

    float setConsumeStaminaTime()
    {
        return 0.1f;
    }

    public void Movement(bool forwardPressed, bool rightPressed, bool leftPressed, bool backPressed, bool runPressed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Normalized so that if two keys are pressed the character doesn't go faster
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        #region change player speed when on block action
        if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("B"))
        {
            playerStats.speed = 2f;
            isSprinting = false;
            _sprinting = false;
        }
        #endregion

        #region Dodge
        if (isDodging)
        {
            DodgeTime = 0.2f;
        }
        if (DodgeTime > 0)
        {
            DodgeTime -= Time.deltaTime;
            rigidbody.AddRelativeForce(Vector3.forward * 20);

        }
        if (DodgeTime <= 0)
        {
            isDodging = false;
            anim.ResetTrigger("Dodge");
        }
        #endregion

        if (forwardPressed)
        {
            Sprint();
            if(!isDodging)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                rigidbody.MovePosition(rigidbody.position + moveDir.normalized * playerStats.speed * Time.fixedDeltaTime);
                _zVel = _sprinting ? 2 : 1;
            }
        }

        if (leftPressed)
        {
            Sprint();
            Vector3 moveVector = this.playerCamera.transform.right.normalized * playerStats.speed;
           _xVel = _sprinting ? -2 : -1;
        }

        
        //if (backPressed && GetComponent<SwordCombat>().isLostBodyBalance == false)
        //{
        //    anim.SetBool("walking", true);
        //    Sprint();
        //    Vector3 moveVector = new Vector3(-camDirection.x * playerStats.speed, 0, -camDirection.z * playerStats.speed);
        //    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveVector.normalized), 0.1f);
        //    ////Debug.Log("pressing S");
        //    this.transform.position += new Vector3(-camDirection.x * playerStats.speed, 0, -camDirection.z * playerStats.speed);
        //
        //   
        //}

        if (rightPressed)
        {
            Sprint();
            Vector3 moveVector = this.playerCamera.transform.right.normalized * playerStats.speed;
            //myController.Move(moveVector * Time.fixedDeltaTime);
           _xVel = _sprinting ? 2 : 1;
        }

        if (!leftPressed && !rightPressed)
        {
            _xVel = 0.0f;
        }

        if (!forwardPressed)
        {
            _zVel = 0.0f;
        }


    }
}