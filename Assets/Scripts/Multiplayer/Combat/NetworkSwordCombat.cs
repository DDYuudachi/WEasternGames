using Mirror;
using UnityEngine;

public class NetworkSwordCombat : NetworkBehaviour
{
   
    
    public AudioSource danger;
    public AudioSource light1;
    public AudioSource light2;
    public AudioSource light3;
    public AudioSource light4;
    public AudioSource heavy1;
    public AudioSource heavy2;
    
    public static bool isEnemyAttack = false;
    bool isPlayerBlock = false;
    private float enemyAttackTimer = 0.0f;
    private float enemyAttackCoolDown = 5.0f;
    private bool isAttackSoundPlaying = false;
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

    NetworkAnimator _anim;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("NetworkPlayer");
        _anim = GetComponent<NetworkAnimator>();
        //capsule = GameObject.Find("Capsule");
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        //playerCamera = GameObject.Find("PlayerCamera");
    }

    // Update is called once per frame
    void Update()
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
            GetComponent<NetworkPlayerStats>().readyToRestoreStaminaTime = GetComponent<NetworkPlayerStats>().setReadyToRestoreStaminaTime();
        }
        if (resetOutOfCombatTime > 0)
        {
            resetOutOfCombatTime -= Time.deltaTime;
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
            if (dummy.GetComponent<Enemy>().HP > 0)
            {
                float EnemyDistance;
                EnemyDistance = Vector3.Distance(player.transform.position, dummy.transform.position);
                //Debug.Log(EnemyDistance);
                if (EnemyDistance < 3 && isEnemyAttack == false)
                {
                    danger.Play();
                    isEnemyAttack = true;
                }
                if (isEnemyAttack == true && enemyAttackTimer <= enemyAttackCoolDown)
                {
                    enemyAttackTimer += Time.deltaTime;
                }
                if (!danger.isPlaying && isEnemyAttack == true && enemyAttackTimer >= enemyAttackCoolDown)
                {
                    enemyAttackTimer = 0;
                    isPlayerBlock = false;
                    isEnemyAttack = false;
                }
                Attack(EnemyDistance);
                KnockBackPlayer(EnemyDistance);
            }
        }
    }

    void Attack(float EnemyDistance)
    {
        #region Play Sound
        if(!light1.isPlaying && !light2.isPlaying && !light3.isPlaying && !light4.isPlaying && !heavy1.isPlaying && !heavy2.isPlaying)
        {
            isAttackSoundPlaying = false;
        }

        if(enemyAttackTimer >= 0.8f && enemyAttackTimer <= 1.2f && Input.GetMouseButtonDown(1) && isAttackSoundPlaying == false && EnemyDistance < 3 && isPlayerBlock == false)
        {
            _anim.SetTrigger("Block");
            isAttackSoundPlaying = true;
            isPlayerBlock = true;
            heavy1.Play();
        }
        else if(enemyAttackTimer > 1.2f && enemyAttackTimer <= 1.5f && Input.GetMouseButtonDown(1) && isAttackSoundPlaying == false && EnemyDistance < 3 && isPlayerBlock == false)
        {
            _anim.SetTrigger("Block");
            isAttackSoundPlaying = true;
            isPlayerBlock = true;
            heavy2.Play();
        }
        else if((enemyAttackTimer < 0.8f || enemyAttackTimer >1.5f) && Input.GetMouseButtonDown(1) && isAttackSoundPlaying == false && EnemyDistance < 3 && isPlayerBlock == false)
        {
            _anim.SetTrigger("Block");
            isAttackSoundPlaying = true;
            isPlayerBlock = true;
            Random ran = new Random();
            int playSoundNo = Random.Range(1, 4);
            if(playSoundNo == 1)
            {
                light1.Play();
                GetComponent<NetworkPlayerStats>().stamina -= 40;
                GetComponent<NetworkPlayerStats>().health -= 25;
                isEncounter = true;
                if (GetComponent<NetworkPlayerStats>().stamina <= 0)
                {
                    player.GetComponent<NetworkPlayerMovement>().isOnKnockBack = true;
                }
            }
            else if(playSoundNo == 2)
            {
                light2.Play();
                GetComponent<NetworkPlayerStats>().stamina -= 40;
                GetComponent<NetworkPlayerStats>().health -= 25;
                isEncounter = true;
                if (GetComponent<NetworkPlayerStats>().stamina <= 0)
                {
                    player.GetComponent<NetworkPlayerMovement>().isOnKnockBack = true;
                }
            }
            else if (playSoundNo == 3)
            {
                light3.Play();
                GetComponent<NetworkPlayerStats>().stamina -= 40;
                GetComponent<NetworkPlayerStats>().health -= 25;
                isEncounter = true;
                if (GetComponent<NetworkPlayerStats>().stamina <= 0)
                {
                    player.GetComponent<NetworkPlayerMovement>().isOnKnockBack = true;
                }
            }
            else if (playSoundNo == 4)
            {
                light4.Play();
                GetComponent<NetworkPlayerStats>().stamina -= 40;
                GetComponent<NetworkPlayerStats>().health -= 25;
                isEncounter = true;
                if (GetComponent<NetworkPlayerStats>().stamina <= 0)
                {
                    player.GetComponent<NetworkPlayerMovement>().isOnKnockBack = true;
                }
            }
        }
        if ((enemyAttackTimer < 0.8f || enemyAttackTimer > 1.5f))
        {
            foreach (GameObject gameObject in enemys)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.grey;
            }
        }
        else if ((enemyAttackTimer >= 0.8f || enemyAttackTimer <= 1.5f))
        {
            foreach (GameObject gameObject in enemys)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
        #endregion
    }

    void KnockBackPlayer(float EnemyDistance)
    {
        if(player.GetComponent<NetworkPlayerMovement>().isOnKnockBack == true)
        {
            player.GetComponent<NetworkPlayerMovement>().isOnKnockBack = false;
            isLostBodyBalance = true;
            knockBackTime = setKnockBackTime();
            resetBodyBalanceTime = setBodyBalanceTime();
        }
        if(knockBackTime > 0)
        {
            knockBackTime -= Time.deltaTime;
            Velocity = knockBackForce;
            Vector3 knockBackVector = -player.transform.forward * knockBackForce;
            player.GetComponent<NetworkPlayerMovement>().myController.Move(knockBackVector);
        }
        if(resetBodyBalanceTime > 0)
        {
            resetBodyBalanceTime -= Time.deltaTime;
        }
        if(resetBodyBalanceTime <= 0 && isLostBodyBalance == true)
        {
            isLostBodyBalance = false;
        }
        //Debug.Log("reset body balance" + resetBodyBalanceTime);
    }

    private float setKnockBackTime()
    {
        return 0.3f;
    }

    public float setBodyBalanceTime()
    {
        return 4.0f;
    }


}