using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class WeaponCollision : MonoBehaviour
{
    public GameObject player;
    public PlayerAction playerAction;
    PlayerStats playerStats;
    PlayerAnimation playerAnimation;
    PlayerControl playerControl;
    GameObject targetEnemy;
    public GameObject floatingText;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAction = this.player.GetComponent<PlayerAction>();
        playerStats = this.player.GetComponent<PlayerStats>();
        playerAnimation = this.player.GetComponent<PlayerAnimation>();
        playerControl = this.player.GetComponent<PlayerControl>();
        this.GetComponent<Collider>().isTrigger = true;

    }

    float damageCalculation(float atk, float criticalCoefficient, float comboCoefficient, bool isHeavyAtk)
    {
        if(!isHeavyAtk)
        {
            float dmg;
            dmg = atk * criticalCoefficient + atk * comboCoefficient;
            return dmg;
        }
        else
        {
            float dmg;
            dmg = 1.5f * atk * (criticalCoefficient + 0.25f) + atk * comboCoefficient;
            return dmg;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            #region Initial Condition
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            EnemyAction enemyAction = collision.gameObject.GetComponent<EnemyAction>();
            EnemyAnimation enemyAnimation = collision.gameObject.GetComponent<EnemyAnimation>();
            bool isInEnemyFOV = enemy.PlayerInFOV(player);
            float comboRestoreTime = 4;

            #region damage
            if (playerStats.criticalCoefficient <= 2)
            {
                playerStats.criticalCoefficient = Random.Range(0.5f, 1.5f);
            }
            float dmg = damageCalculation(playerStats.baseAtk, playerStats.criticalCoefficient, playerStats.comboCoefficient, playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT"));
            float blockDmg = Random.Range(2.5f, 7.5f);
            if (floatingText || enemy)
            {
            //    showDmgText(dmg, enemy);
            }
            #endregion
            #endregion

            #region Enemy Blocking Collision Logic
            // enemy is blocking and get hit by player
            if (this.GetComponent<Collider>().isTrigger == false &&
                enemyAction.isKeepBlocking == true &&
                enemyAction.isPerfectBlock == false &&
                enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("B"))
            {
                #region get player heavy attack
                if (enemy.hitStunValue > 0 && 
                    playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT"))
                {
                    playerStats.hitStunValue -= 100;
                    if (playerStats.hitStunValue <= 0)
                    {
                        enemy.DecreaseHPStamina(dmg, dmg);
                        enemy.readyToRestoreStaminaTime = 5.0f;
                        //enemyAnimation._anim.ResetTrigger("isInjured");
                        //enemyAnimation._anim.SetTrigger("isInjured");
                        playerStats.comboHitCount++;
                        playerStats.comboHitRestoreTime = comboRestoreTime;
                        if(playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                        {
                            playerStats.comboDamageAdd = true;
                        }
                        showDmgText(dmg, enemy);
                    }
                }
                #endregion

                #region get player light attack
                //get enemy light attack
                if (enemy.hitStunValue > 0 &&
                    playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT") &&
                    isInEnemyFOV)
                {
                    enemy.hitStunValue -= 10;
                    enemy.hitStunRestoreSecond = 5.0f;
                    enemy.readyToRestoreStaminaTime = 5.0f;

                    if (enemy.hitStunValue > 0)
                    {
                        enemy.DecreaseHPStamina(blockDmg, blockDmg);
                        enemyAnimation._anim.ResetTrigger("isGetBlockingImpact");
                        enemyAnimation._anim.SetTrigger("isGetBlockingImpact");
                        showDmgText(blockDmg, enemy);
                        // spawn sword clash effect
                        enemy.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
                    }

                    else if (enemy.hitStunValue <= 0)
                    {
                        enemy.DecreaseHPStamina(dmg, dmg);
                        enemy.readyToRestoreStaminaTime = 5.0f;
                        //enemyAnimation._anim.ResetTrigger("isInjured");
                        //_anim.SetTrigger("isInjured");
                        showDmgText(dmg, enemy);
                    }
                }
                else if (enemy.hitStunValue > 0 &&
                    playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT") &&
                    !isInEnemyFOV)
                {
                    enemy.DecreaseHPStamina(dmg, dmg);
                    showDmgText(dmg, enemy);
                    enemy.readyToRestoreStaminaTime = 5.0f;
                    //enemyAnimation._anim.ResetTrigger("isInjured");
                    //enemyAnimation._anim.SetTrigger("isInjured");
                }
                #endregion
                playerStats.comboHitCount++;
                playerStats.comboHitRestoreTime = comboRestoreTime;
                if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                {
                    playerStats.comboDamageAdd = true;
                }
            }

            // enemy is in blocking impact status and get hit
            if (this.GetComponent<Collider>().isTrigger = false &&
                enemyAction.isKeepBlocking == true &&
                enemyAction.isPerfectBlock == false &&
                enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("BI"))
            {
                if(isInEnemyFOV)
                {
                    enemy.hitStunValue -= 20;
                    enemyAnimation._anim.ResetTrigger("isGetBlockingImpact");
                    enemyAnimation._anim.SetTrigger("isGetBlockingImpact");
                    this.GetComponent<Collider>().isTrigger = true;

                    // spawn sword clash effect
                    enemy.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
                    playerStats.comboHitCount++;
                    playerStats.comboHitRestoreTime = comboRestoreTime;
                    if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                    {
                        playerStats.comboDamageAdd = true;
                    }
                }
                else
                {
                    if(playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT"))
                    {
                        this.GetComponent<Collider>().isTrigger = true;
                        enemy.DecreaseHPStamina(dmg, dmg); 
                        enemy.readyToRestoreStaminaTime = 5.0f;
                        showDmgText(dmg, enemy);
                        //enemyAnimation._anim.ResetTrigger("isInjured");
                        //enemyAnimation._anim.SetTrigger("isInjured");
                    }
                    else if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT"))
                    {
                        this.GetComponent<Collider>().isTrigger = true;
                        enemy.DecreaseHPStamina(dmg, dmg);
                        showDmgText(dmg, enemy);
                        enemy.readyToRestoreStaminaTime = 5.0f;
                        //enemyAnimation._anim.ResetTrigger("isInjured");
                        //enemyAnimation._anim.SetTrigger("isInjured");
                    }
                    playerStats.comboHitCount++;
                    playerStats.comboHitRestoreTime = comboRestoreTime;
                    if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                    {
                        playerStats.comboDamageAdd = true;
                    }
                }
            }
            #endregion
            //
            // enemy is not in block action and get hit by player (Heavy attack)
            if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT") &&
               this.GetComponent<Collider>().isTrigger == false &&
               enemyAction.isKeepBlocking == false &&
               enemyAction.isPerfectBlock == false)
            {
                this.GetComponent<Collider>().isTrigger = true;
                enemy.DecreaseHPStamina(dmg, dmg);
                showDmgText(dmg, enemy);
                enemy.readyToRestoreStaminaTime = 5.0f;
                //playerMovement.isSprinting = false;
                //enemyAnimation._anim.ResetTrigger("isInjured");
                //enemyAnimation._anim.SetTrigger("isInjured");
                playerStats.comboHitCount++;
                playerStats.comboHitRestoreTime = comboRestoreTime;
                if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                {
                    playerStats.comboDamageAdd = true;
                }
            }

            // enemy is not in block action and get hit by player  (light attack)
            else if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT") &&
                     this.GetComponent<Collider>().isTrigger == false &&
                     enemy.GetComponent<EnemyAction>().isKeepBlocking == false &&
                     enemyAction.isPerfectBlock == false)
            {
                this.GetComponent<Collider>().isTrigger = true;
                enemy.DecreaseHPStamina(dmg, dmg);
                showDmgText(dmg, enemy);
                enemy.readyToRestoreStaminaTime = 5.0f;
                //enemyAnimation._anim.ResetTrigger("isInjured");
                //enemyAnimation._anim.SetTrigger("isInjured");
                playerStats.comboHitCount++;
                playerStats.comboHitRestoreTime = comboRestoreTime;
                if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                {
                    playerStats.comboDamageAdd = true;
                }
            }

            // enemy is in perfect block Transistion but not in perfect block timing (Heavy attack)
            // (GetCurrentAnimatorStateInfo(0).IsTag("PB")) get current animator state by tag https://forum.unity.com/threads/current-animator-state-name.331803/
            else if ((enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PB") ||
                enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("A") ||
                enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PBO")) &&
                enemyAction.isPerfectBlock == false &&
                playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT"))
            {
                enemy.DecreaseHPStamina(dmg, dmg);
                showDmgText(dmg, enemy);
                enemy.readyToRestoreStaminaTime = 5.0f;
                //enemyAnimation._anim.ResetTrigger("isInjured");
                //enemyAnimation._anim.SetTrigger("isInjured");
                playerStats.comboHitCount++;
                playerStats.comboHitRestoreTime = comboRestoreTime;
                if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                {
                    playerStats.comboDamageAdd = true;
                }
                this.GetComponent<Collider>().isTrigger = true;
            }

            // enemy is in perfect block Transistion but not in perfect block timing (light attack)
            else if ((enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PB") ||
                      enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("A") ||
                      enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PBO")) &&
                      enemyAction.isPerfectBlock == false &&
                      enemyAction.isKeepBlocking == true &&
                      playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT"))
            {
                if(isInEnemyFOV)
                {
                    enemy.DecreaseHPStamina(dmg, dmg);
                    showDmgText(dmg, enemy);
                    enemy.hitStunValue -= 5;
                    enemyAnimation._anim.ResetTrigger("isGetBlockingImpact");
                    enemyAnimation._anim.SetTrigger("isGetBlockingImpact");
                    enemy.readyToRestoreStaminaTime = 5.0f;
                    //playerMovement.isSprinting = false;
                    this.GetComponent<Collider>().isTrigger = true;
                    enemy.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
                    playerStats.comboHitCount++;
                    playerStats.comboHitRestoreTime = comboRestoreTime;
                    if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                    {
                        playerStats.comboDamageAdd = true;
                    }
                }
                else
                {
                    this.GetComponent<Collider>().isTrigger = true;
                    enemy.DecreaseHPStamina(dmg, dmg);
                    showDmgText(dmg, enemy);
                    enemy.readyToRestoreStaminaTime = 5.0f;
                    //enemyAnimation._anim.ResetTrigger("isInjured");
                    //enemyAnimation._anim.SetTrigger("isInjured");
                    playerStats.comboHitCount++;
                    playerStats.comboHitRestoreTime = comboRestoreTime;
                    if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                    {
                        playerStats.comboDamageAdd = true;
                    }
                }
            }

            else if (enemyAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("GH"))
            {
                if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT"))
                {
                    enemy.DecreaseHPStamina(dmg, dmg);
                    showDmgText(dmg, enemy);
                }
                else if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT"))
                {
                    enemy.DecreaseHPStamina(dmg, dmg);
                    showDmgText(dmg, enemy);
                }
                //enemyAnimation._anim.ResetTrigger("isInjured");
                //enemyAnimation._anim.SetTrigger("isInjured");
                enemy.readyToRestoreStaminaTime = 5.0f;
                playerStats.comboHitCount++;
                playerStats.comboHitRestoreTime = comboRestoreTime;
                if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                {
                    playerStats.comboDamageAdd = true;
                }
            }

            // enemy is in perfect block
            if (collision.gameObject.GetComponent<EnemyAction>().isPerfectBlock == true)
            {
                if(isInEnemyFOV)
                {
                    playerAnimation._anim.ResetTrigger("isGetEnemyPerfectBlock");
                    playerAnimation._anim.SetTrigger("isGetEnemyPerfectBlock");
                    playerAction.isPlayerAttacking = false;
                    playerStats.isHitStun = true;

                    // spawn sword clash effect
                    collision.gameObject.GetComponentInParent<SwordEffectSpawner>().SpawnBigSwordClash();
                    playerControl.comboHit = 0;
                    playerControl.comboValidTime = 0;
                }
                else
                {
                    if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT"))
                    {
                        enemy.DecreaseHPStamina(dmg, dmg);
                        showDmgText(dmg, enemy);
                    }
                    else if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT"))
                    {
                        enemy.DecreaseHPStamina(dmg, dmg);
                        showDmgText(dmg, enemy);
                    }
                    //enemyAnimation._anim.ResetTrigger("isInjured");
                    //enemyAnimation._anim.SetTrigger("isInjured");
                    enemy.readyToRestoreStaminaTime = 5.0f;
                    playerStats.comboHitCount++;
                    playerStats.comboHitRestoreTime = comboRestoreTime;
                    if (playerStats.comboHitCount % 5 == 0 && playerStats.comboHitCount != 0)
                    {
                        playerStats.comboDamageAdd = true;
                    }
                }
            }
           // this.GetComponent<Collider>().isTrigger = true;
        }
    }

    public void showDmgText(float dmg, Enemy enemy)
    {
        var text = Instantiate(floatingText, transform.position, Quaternion.identity, enemy.transform);
        text.GetComponent<TextMesh>().text = Mathf.RoundToInt(dmg).ToString();
    }
}

