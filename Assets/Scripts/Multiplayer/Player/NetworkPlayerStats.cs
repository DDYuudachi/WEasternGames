using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerStats : MonoBehaviour
{
    #region Player Stats
    public float health;
    public int stamina;
    public float speed;
    public HP hpUI;
    public Stamina staminaUI;
    #endregion

    #region Trigger
    public float readyToRestoreStaminaTime = 0;
    private float RestoreStaminaTime = 0;
    private bool isRestoreStamina = false;
    #endregion


    void Start()
    {
        health = 100;
        stamina = 100;
        speed = 4;
        
        //hpUI.SetMaxHP(health);
        //staminaUI.SetMaxStaminaSlider(stamina);
    }

    void Update()
    {
        restoreStamina();
        loseCondition();
        //setHealthUI();
        //setStaminaUI();
    }

    void setStaminaUI()
    {
        staminaUI.setStaminaSlider(stamina);
    }

    void setHealthUI()
    {
        hpUI.setHealth(health);
    }

    void loseCondition()
    {
        if (health <= 0)
        {
            GetComponent<SwordCombat>().enabled = false;
            GetComponent<NetworkPlayerBehaviour>().enabled = false;
            GetComponent<NetworkPlayerMovement>().enabled = false;
            GetComponent<PlayerAction>().enabled = false;
        }
    }

    void restoreStamina()
    {
        if (GetComponent<NetworkPlayerBehaviour>().isOnLightAction == false && GetComponent<NetworkPlayerBehaviour>().isOnHeavyAction == false
            && GetComponent<NetworkPlayerMovement>().isSprinting == false && GetComponent<SwordCombat>().isOnCombat == false)
        {
            if(readyToRestoreStaminaTime > 0) // Time preparation before restore stamina
            {
                readyToRestoreStaminaTime -= Time.deltaTime;
                isRestoreStamina = false;
            }
            if (readyToRestoreStaminaTime <= 0) // Time preparation before restore stamina
            {
                isRestoreStamina = true;
            }
            if (isRestoreStamina == true)
            {
                if (RestoreStaminaTime > 0)
                {
                    RestoreStaminaTime -= Time.deltaTime;
                }
                if (RestoreStaminaTime <= 0 && stamina <= 100)
                {
                    stamina += 2;
                    if (stamina >= 100)
                    {
                        stamina = 100;
                    }
                    if(stamina > 0)
                    {
                        GetComponent<NetworkPlayerMovement>().isOnKnockBack = false;
                    }
                    RestoreStaminaTime = setRestoreStaminaTime();
                }
            }      
        }
        if (stamina <= 0)
        {
            stamina = 0;
            speed = 4;
            GetComponent<NetworkPlayerMovement>()._sprinting = false;
        }
        //Debug.Log(readyToRestoreStaminaTime);
    }

    public float setReadyToRestoreStaminaTime()
    {
        return 2.5f;
    }
    private float setRestoreStaminaTime()
    {
        return 0.2f;
    }
}
