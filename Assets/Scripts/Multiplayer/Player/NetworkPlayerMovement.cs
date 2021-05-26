using Mirror;
using UnityEngine;

public class NetworkPlayerMovement : NetworkBehaviour
{
    public Transform playerCamera;
    private Animator anim;

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

    //Jump Setting
    public float verticalVelocity;
    private float gravity = 10f;
    private float jumpForce = 5f;

    //private Transform CameraPivot; //empty point on player
    public CharacterController myController;

    //private Vector3 camDirection;
    public bool isOnKnockBack = false;

    //Sprint
    public bool isSprinting = false;
    private bool isAcceleratedFinished = false;
    private bool isAcclerationCoolDownOn = false;
    private float acclerationTime = 0.2f;
    private float acclerationCoolDown = 0.5f;
    private float consumeStaminaSpeedTime = 0;

    //Remove later once network implementation is finished
    public bool player2;

    // Start is called before the first frame update
    void Start()
    {
       myController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();

        _zVelHash = Animator.StringToHash("velZ");
        _xVelHash = Animator.StringToHash("velX");
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if(!isOnKnockBack){Movement(forwardPressed, rightPressed, leftPressed, backPressed, runPressed);}
        
        
        
        Jump();

        anim.SetFloat(_xVelHash, _xVel);
        anim.SetFloat(_zVelHash, _zVel);
    }

    private void Sprint()
    {
        if (GetComponent<NetworkPlayerBehaviour>().isOnLightAction == false &&
            GetComponent<NetworkPlayerBehaviour>().isOnHeavyAction == false && GetComponent<NetworkPlayerStats>().stamina > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isSprinting = true;
                isAcclerationCoolDownOn = true;
            }

            if (isAcclerationCoolDownOn == true && acclerationCoolDown >= 0) // accleration cool down
            {
                acclerationCoolDown -= Time.deltaTime;
            }

            if (isAcclerationCoolDownOn == true && acclerationTime >= 0) // player instant accleration 
            {
                _sprinting = true;
                GetComponent<NetworkPlayerStats>().speed = 15f;
                acclerationTime -= Time.deltaTime;
            }

            if (acclerationCoolDown <= 0)
            {
                acclerationTime = 0.2f;
                acclerationCoolDown = 0.5f;
                isAcclerationCoolDownOn = false;
                isAcceleratedFinished = true;
            }

            if (Input.GetKey(KeyCode.LeftShift) && isAcceleratedFinished == true)
            {
                GetComponent<NetworkPlayerStats>().speed = 8f;
            }

            if (isSprinting == true)
            {
                GetComponent<NetworkPlayerStats>().readyToRestoreStaminaTime =
                    GetComponent<NetworkPlayerStats>().setReadyToRestoreStaminaTime();
                if (consumeStaminaSpeedTime <= 0)
                {
                    GetComponent<NetworkPlayerStats>().stamina -= 2;
                    consumeStaminaSpeedTime = setConsumeStaminaTime();
                }

                if (consumeStaminaSpeedTime > 0 && this.transform.hasChanged == true)
                {
                    consumeStaminaSpeedTime -= Time.deltaTime;
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || GetComponent<NetworkPlayerStats>().stamina == 0)
            {
                _sprinting = false;
                GetComponent<NetworkPlayerStats>().speed = 4f;
                isAcceleratedFinished = false;
                isSprinting = false;
                consumeStaminaSpeedTime = 0.1f;
            }
        }
    }

    float setConsumeStaminaTime()
    {
        return 0.1f;
    }

    public void Jump()
    {
        if (myController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime; //have a little pressure on player to stick to the floor
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 jumpVector = new Vector3(0, verticalVelocity, 0);
        myController.Move(jumpVector * Time.deltaTime);
    }

    public void Movement(bool forwardPressed, bool rightPressed, bool leftPressed, bool backPressed, bool runPressed)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //Normalized so that if two keys are pressed the character doesn't go faster
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        /*
         * change player speed when on heavy attack
         */
        if (GetComponent<NetworkPlayerBehaviour>().isOnHeavyAction == true)
        {
            GetComponent<NetworkPlayerStats>().speed = 0;
        }

        /*
         * change player speed when on light attack
         */
        if (GetComponent<NetworkPlayerBehaviour>().isOnLightAction == true)
        {
            GetComponent<NetworkPlayerStats>().speed = 1f;
        }

        if (GetComponent<NetworkPlayerBehaviour>().isOnHeavyAction == false &&
            GetComponent<NetworkPlayerBehaviour>().isOnLightAction == false)
        {
            GetComponent<NetworkPlayerStats>().speed = 4f;
        }


        if (forwardPressed && GetComponent<SwordCombat>().isLostBodyBalance == false)
        {
            Sprint();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle =
                Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            myController.Move(moveDir.normalized * GetComponent<NetworkPlayerStats>().speed * Time.deltaTime);
            _zVel = _sprinting ? 2 : 1;
        }

        if (leftPressed && GetComponent<SwordCombat>().isLostBodyBalance == false)
        {
            Sprint();
            Vector3 moveVector = -Camera.main.transform.right.normalized * GetComponent<NetworkPlayerStats>().speed;
            myController.Move(moveVector * Time.deltaTime);
            _xVel = _sprinting ? -2 : -1;
        }

        if (rightPressed && GetComponent<SwordCombat>().isLostBodyBalance == false)
        {
            Sprint();
            Vector3 moveVector = Camera.main.transform.right.normalized * GetComponent<NetworkPlayerStats>().speed;
            myController.Move(moveVector * Time.deltaTime);
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
