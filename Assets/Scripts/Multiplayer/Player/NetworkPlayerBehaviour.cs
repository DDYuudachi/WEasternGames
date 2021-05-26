using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkPlayerBehaviour : NetworkBehaviour
{
    ActionType playerAction;
    private NetworkAnimator _anim;
    
    public GameObject weapon;

    #region Attack Status Check
    //public bool isOnAttackAction = false;
    public bool isOnLightAction = false;
    public bool isOnHeavyAction = false;
    private bool beforeDoATK = false;
    private bool duringDoATK = false;
    private bool afterDoATK = false;
    public static bool isLightHit = false;
    public static bool isHeavyHit = false;
    #endregion

    #region Light Attack
    float beforeLAtkTime = 0.3f;
    float duringLAtkTime = 0.001f;
    float afterLAtkTime = 1.0f;
    #endregion

    #region Heavy Attack
    float beforeHAtkTime = 0.1f;
    float duringHAtkTime = 0.5f;
    float afterHAtkTime = 1.0f;
    #endregion

    void Start()
    {
        _anim = GetComponent<NetworkAnimator>();
        playerAction = GetComponent<PlayerAction>().action;
    }

    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        playerAction = GetComponent<PlayerAction>().action;

        #region Debug
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Debug.Log(playerAction);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetComponent<PlayerAction>().action = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetComponent<PlayerAction>().action = 0;
        }
        #endregion

        switch (playerAction)
        {
            case ActionType.Idle:
                //Debug.Log(playerAction);
                break;

            case ActionType.LightAttack:
                isOnLightAction = true;
                doLightAttack();
                break;

            case ActionType.HeavyAttack:
                isOnHeavyAction = true;
                doHeavyAttack();
                break;
        }
    }

    private void doHeavyAttack()
    {
        if (isOnHeavyAction == true)
        {
            if (beforeHAtkTime > 0 && beforeDoATK == false) // before do Action
            {
               beforeHAtkTime -= Time.deltaTime;
            }
            if (beforeHAtkTime <= 0 && beforeDoATK == false) //check before do atk action is finished
            {
                GetComponent<NetworkPlayerStats>().stamina -= 10;
                GetComponent<NetworkPlayerStats>().readyToRestoreStaminaTime = GetComponent<NetworkPlayerStats>().setReadyToRestoreStaminaTime();
                //Debug.Log("Before Action is Done");
                // Debug.Log(GetComponent<PlayerStats>().stamina);
                beforeDoATK = true;
            }

            if (beforeDoATK == true && duringDoATK == false) // do Action
            {
                if (duringHAtkTime > 0 && duringDoATK == false) // doing attack action
                {
                   
                    if (duringHAtkTime >= 0.5f && duringHAtkTime <= 0.7f)
                    {
                        isHeavyHit = true;
                        _anim.SetTrigger("Heavy Attack");
                    }

                    duringHAtkTime -= Time.deltaTime;
                }
                if (duringHAtkTime <= 0 && duringDoATK == false)
                {
                    //Debug.Log("Doing Action is Done");
                    duringDoATK = true;
                }
            }

            if (duringDoATK == true && afterDoATK == false) // finished one loop of action
            {
                if (afterHAtkTime > 0 && afterDoATK == false)
                {
                    afterHAtkTime -= Time.deltaTime;
                }
                if (afterHAtkTime <= 0 && afterDoATK == false)
                {
                    //Debug.Log("After Action is Done");
                    afterDoATK = true;
                }
            }
        }

        if (afterDoATK == true || isOnHeavyAction == false) //reset all values
        {
            beforeHAtkTime = 0.1f;
            duringHAtkTime = 0.5f;
            afterHAtkTime = 1.0f;
            isOnHeavyAction = false;
            beforeDoATK = false;
            duringDoATK = false;
            afterDoATK = false;
            GetComponent<PlayerAction>().action = ActionType.Idle;
            isHeavyHit = false;
        }
    }

    private void doLightAttack()
    {
        if(isOnLightAction == true)
        {
            if (beforeLAtkTime > 0 && beforeDoATK == false) // before do Action
            {
               beforeLAtkTime -= Time.deltaTime;
            }
            if (beforeLAtkTime <= 0 && beforeDoATK == false) //check before do atk action is finished
            {
                GetComponent<NetworkPlayerStats>().stamina -= 5;
                //GetComponent<PlayerStats>().readyToRestoreStaminaTime = GetComponent<PlayerStats>().setReadyToRestoreStaminaTime();
                //Debug.Log("Before Action is Done");
                beforeDoATK = true;
            }
           
            if (beforeDoATK == true && duringDoATK == false) // do Action
            {
                
                if (duringLAtkTime > 0 && duringDoATK == false) // doing attack action
                {
                    isLightHit = true;
                    _anim.SetTrigger("Light Attack");

                    duringLAtkTime -= Time.deltaTime;
                }
                if (duringLAtkTime <= 0 && duringDoATK == false)
                {
                    //Debug.Log("Doing Action is Done");
                    duringDoATK = true;
                }
            }

            if (duringDoATK == true && afterDoATK == false) // finished one loop of action
            {
                if (afterLAtkTime > 0 && afterDoATK == false)
                {
                    afterLAtkTime -= Time.deltaTime;
                }
                if (afterLAtkTime <= 0 && afterDoATK == false)
                {
                    //Debug.Log("After Action is Done");
                    afterDoATK = true;
                }
            }
        }

        if (afterDoATK == true || isOnLightAction == false) //reset all values
        {
            beforeLAtkTime = 0.3f;
            duringLAtkTime = 0.001f;
            afterLAtkTime = 1.0f;
            isOnLightAction = false;
            beforeDoATK = false;
            duringDoATK = false;
            afterDoATK = false;
            GetComponent<PlayerAction>().action = (int)ActionType.Idle;
            isLightHit = false;
        }
    }
}
