using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class SwordCombat : MonoBehaviour
{
   
    
    public AudioSource danger;
    public AudioSource light1;
    public AudioSource light2;
    public AudioSource light3;
    public AudioSource light4;
    public AudioSource heavy1;
    public AudioSource heavy2;
    
    public static bool isEnemyAttack = false;
    private float enemyAttackTimer = 0.0f;
    private float enemyAttackCoolDown = 5.0f;
    //private Vector3 camDirection;
    private GameObject player;
    //private GameObject capsule;
    private GameObject[] enemys;
    //private GameObject playerCamera;
    private float knockBackTime = 0.0f;
    private float knockBackForce = 0.5f;
    private float Velocity;

    public bool isEncounter = false; //used for restore stamina
    public bool isOnCombat = false;
    public float resetOutOfCombatTime = 0;
    public float resetBodyBalanceTime = 0;
    public bool isLostBodyBalance = false;

    public bool normalBlockFromHeavyAttack = false;
    public bool isStepBack = false;

    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        _anim = player.GetComponent<Animator>();
        //capsule = GameObject.Find("Capsule");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //playerCamera = GameObject.Find("PlayerCamera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DetectAttack();
        ResetOutOfCombat();
        //Debug.Log(enemyAttackTimer);
    }

    void ResetOutOfCombat()
    {
        if (isEncounter == true)
        {
            resetOutOfCombatTime = setOutOfCombatTime();
            isEncounter = false;
            isOnCombat = true;
            GetComponent<PlayerStats>().readyToRestoreStaminaTime = GetComponent<PlayerStats>().setReadyToRestoreStaminaTime(3.0f);
        }
        if (resetOutOfCombatTime > 0)
        {
            resetOutOfCombatTime -= Time.fixedDeltaTime;
        }
        if (resetOutOfCombatTime <= 0)
        {
            isOnCombat = false;
        }
        //Debug.Log(resetOutOfCombatTime);
    }
    
    public float setOutOfCombatTime()
    {
        return 3f;
    }

    void DetectAttack()
    {
        foreach(GameObject dummy in enemys)
        {
            //if (dummy.GetComponent<Enemy>().HP > 0)
            {
                float EnemyDistance;
                EnemyDistance = Vector3.Distance(player.transform.position, dummy.transform.position);
                //Debug.Log(EnemyDistance);
                if (EnemyDistance < 3 && isEnemyAttack == false)
                {
                    //danger.Play();
                    isEnemyAttack = true;
                }
                if (isEnemyAttack == true && enemyAttackTimer <= enemyAttackCoolDown)
                {
                    enemyAttackTimer += Time.fixedDeltaTime;
                }
                if ( isEnemyAttack == true && enemyAttackTimer >= enemyAttackCoolDown)
                {
                    enemyAttackTimer = 0;
                    isEnemyAttack = false;
                }
                Attack();
                normalBlockKnockBackFromHeavyAttack(2);
                //KnockBackPlayer();
            }
        }
    }

    void Attack()
    {
        if (GetComponent<PlayerStats>().stamina <= 0)
        {
            player.GetComponent<PlayerMovement>().isOnKnockBack = true;
        }
        //#region Play Sound
        //if(!light1.isPlaying && !light2.isPlaying && !light3.isPlaying && !light4.isPlaying && !heavy1.isPlaying && !heavy2.isPlaying)
        //{
        //    isAttackSoundPlaying = false;
        //}

        //if(enemyAttackTimer >= 0.8f && enemyAttackTimer <= 1.2f && GetComponent<PlayerAction>().PlayerStatus == (int)ActionType.InstantBlock && isAttackSoundPlaying == false && EnemyDistance < 3 && isPlayerBlock == false)
        //{
        //    isAttackSoundPlaying = true;
        //    isPlayerBlock = true;
        //    heavy1.Play();
        //}
        //else if(enemyAttackTimer > 1.2f && enemyAttackTimer <= 1.5f && GetComponent<PlayerAction>().PlayerStatus == (int)ActionType.InstantBlock && isAttackSoundPlaying == false && EnemyDistance < 3 && isPlayerBlock == false)
        //{
        //    isAttackSoundPlaying = true;
        //    isPlayerBlock = true;
        //    heavy2.Play();
        //}
        //else if((enemyAttackTimer < 0.8f || enemyAttackTimer >1.5f) && (GetComponent<PlayerAction>().PlayerStatus == (int)ActionType.LongBlock || GetComponent<PlayerAction>().PlayerStatus == (int)ActionType.InstantBlock) 
        //            && isAttackSoundPlaying == false && EnemyDistance < 3 && isPlayerBlock == false)
        //{
        //    isAttackSoundPlaying = true;
        //    isPlayerBlock = true;
        //    Random ran = new Random();
        //    int playSoundNo = Random.Range(1, 4);
        //    if(playSoundNo == 1)
        //    {
        //        light1.Play();
        //        GetComponent<PlayerStats>().stamina -= 40;
        //        GetComponent<PlayerStats>().health -= 25;
        //        isEncounter = true;
        //        normalBlockFromHeavyAttack = true;
        //        if (GetComponent<PlayerStats>().stamina <= 0)
        //        {
        //            player.GetComponent<PlayerMovement>().isOnKnockBack = true;
        //        }
        //    }
        //    else if(playSoundNo == 2)
        //    {
        //        light2.Play();
        //        GetComponent<PlayerStats>().stamina -= 40;
        //        GetComponent<PlayerStats>().health -= 25;
        //        isEncounter = true;
        //        normalBlockFromHeavyAttack = true;
        //        if (GetComponent<PlayerStats>().stamina <= 0)
        //        {
        //            player.GetComponent<PlayerMovement>().isOnKnockBack = true;
        //        }
        //    }
        //    else if (playSoundNo == 3)
        //    {
        //        light3.Play();
        //        GetComponent<PlayerStats>().stamina -= 40;
        //        GetComponent<PlayerStats>().health -= 25;
        //        isEncounter = true;
        //        normalBlockFromHeavyAttack = true;
        //        if (GetComponent<PlayerStats>().stamina <= 0)
        //        {
        //            player.GetComponent<PlayerMovement>().isOnKnockBack = true;
        //        }
        //    }
        //    else if (playSoundNo == 4)
        //    {
        //        light4.Play();
        //        GetComponent<PlayerStats>().stamina -= 40;
        //        GetComponent<PlayerStats>().health -= 25;
        //        isEncounter = true;
        //        normalBlockFromHeavyAttack = true;

        //    }

    }

    //void KnockBackPlayer()
    //{
    //    if(player.GetComponent<PlayerMovement>().isOnKnockBack == true)
    //    {
    //        player.GetComponent<PlayerMovement>().isOnKnockBack = false;
    //        isLostBodyBalance = true;
    //        knockBackTime = setKnockBackTime();
    //        resetBodyBalanceTime = setBodyBalanceTime();
    //    }
    //    if(knockBackTime > 0)
    //    {
    //        knockBackTime -= Time.fixedDeltaTime;
    //        Velocity = knockBackForce;
    //        Vector3 knockBackVector = -player.transform.forward * knockBackForce;
    //        player.GetComponent<PlayerMovement>().myController.Move(knockBackVector);
    //    }
    //    if(resetBodyBalanceTime > 0)
    //    {
    //        resetBodyBalanceTime -= Time.fixedDeltaTime;
    //    }
    //    if(resetBodyBalanceTime <= 0 && isLostBodyBalance == true)
    //    {
    //        isLostBodyBalance = false;
    //    }
    //    //Debug.Log("reset body balance" + resetBodyBalanceTime);
    //}

    private float setKnockBackTime()
    {
        return 0.3f;
    }

    public float setBodyBalanceTime()
    {
        return 4.0f;
    }

    void normalBlockKnockBackFromHeavyAttack(float forceVelocity)
    {
        if (normalBlockFromHeavyAttack == true)
        {
            normalBlockFromHeavyAttack = false;
            isStepBack = true;
            knockBackTime = setStepBackTime();
            resetBodyBalanceTime = setRestoreStepBackBalanceTime();
        }
        if (knockBackTime > 0)
        {
             knockBackTime -= Time.fixedDeltaTime;
             Velocity = forceVelocity;
             Vector3 knockBackVector = -player.transform.forward * knockBackForce;
             player.GetComponent<PlayerMovement>().myController.Move(knockBackVector);
        }
        if (resetBodyBalanceTime > 0)
        {
             resetBodyBalanceTime -= Time.fixedDeltaTime;
        }
        if (resetBodyBalanceTime <= 0 && isStepBack == true)
        {
            isStepBack = false;
            normalBlockFromHeavyAttack = false;
        }
        
        //Debug.Log("reset body balance" + resetBodyBalanceTime);
    }

    private float setStepBackTime()
    {
        return 0.1f;
    }

    public float setRestoreStepBackBalanceTime()
    {
        return 3f;
    }


}