﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SachiAnimation : MonoBehaviour
{
    public Animator _anim;
    public RuntimeAnimatorController runtimeAnimatorController;
    public GameObject katanaObject;
    private PlayerAction playerAction;
    private PlayerControl playerControl;
    private PlayerJump playerJump;
    private DoubleJump doubleJump;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    private AnimatorClipInfo[] clipInfo;
    private Collider collider;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = runtimeAnimatorController; //Load controller at runtime https://answers.unity.com/questions/1243273/runtimeanimatorcontroller-not-loading-from-script.html
        playerAction = GetComponent<PlayerAction>();
        playerControl = GetComponent<PlayerControl>();
        playerJump = GetComponent<PlayerJump>();
        doubleJump = GetComponent<DoubleJump>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        clipInfo = _anim.GetCurrentAnimatorClipInfo(0); // get name of current animation state   https://stackoverflow.com/questions/34846287/get-name-of-current-animation-state

        collider = katanaObject.GetComponent<BoxCollider>(); // to find a child game object by name   //https://docs.unity3d.com/ScriptReference/Transform.Find.html
    }

    void FixedUpdate()
    {
        initialiseAnimatorBool();
        stopDodging();
    }


    private void stopDodging()
    {
        if(_anim.GetCurrentAnimatorStateInfo(0).IsTag("GH") || _anim.GetCurrentAnimatorStateInfo(0).IsTag("GEPB") || _anim.GetCurrentAnimatorStateInfo(0).IsTag("BI"))
        {
            playerMovement.isDodging = false;
        }
    }

    void initialiseAnimatorBool()
    {
        #region Player Block
        _anim.SetBool("isKeepBlocking", playerAction.isKeepBlocking);
        _anim.SetBool("isPerfectBlock", playerAction.isPerfectBlock);
        _anim.SetBool("isAttackTriggered", collider.isTrigger);
        #endregion
        #region Jump
        _anim.SetBool("isFirstJump", playerJump.isJump);
        _anim.SetBool("isSecondJump", doubleJump.isDoubleJump);
        _anim.SetBool("isFalling", playerJump.isFalling);
        _anim.SetBool("isGrounded", playerJump.isGrounded);
        _anim.SetBool("FallingToGround", playerJump.fallingToGround);
        _anim.SetInteger("jumpTimes", playerJump.jumpTimes);
        #endregion
        #region Sprint
        _anim.SetBool("isSprinting", playerMovement.isSprinting);
        _anim.SetBool("isDodging", playerMovement.isDodging);
        _anim.SetBool("isHitStun", playerStats.isHitStun);
        _anim.SetBool("isBlockStun", playerStats.isBlockStun);
        _anim.SetBool("moveKeyPressed", playerMovement.moveKeyPressed);
        #endregion
    }

    #region Player Block Logic
    public void OnAnimation_isPerfectBlock()
    {
        playerAction.isPerfectBlock = true;
    }

    public void OnAnimation_isPerfectBlockEnd()
    {
        playerAction.isPerfectBlock = false;
    }

    public void OnAnimation_isBlockStart()
    {
        playerAction.isKeepBlocking = true;
    }

    public void OnAnimation_isBlockEnd()
    {
        playerAction.isKeepBlocking = false;
    }
    #endregion


    #region Player Attack Logic 
    public void OnAnimation_IsHeavyAttackActive()
    {
        collider.isTrigger = false;
    }

    public void OnAnimation_IsHeavyAttackDeactive()
    {
        collider.isTrigger = true;
    }

    public void OnAnimation_isHeavyAttacking()
    {

    }

    public void OnAnimation_isHeavyAttackingEnd()
    {
        playerAction.isPlayerAttacking = false;
    }

    public void OnAnimation_IsLightAttackActive()
    {
        collider.isTrigger = false;
    }

    public void OnAnimation_IsLightAttackDeactive()
    {
        collider.isTrigger = true;
    }

    public void OnAnimation_isLightAttacking()
    {

    }

    public void OnAnimation_isLightAttackingEnd()
    {
        playerAction.isPlayerAttacking = false;
    }
    #endregion

    #region Player Get Hurt Logic
    public void OnAnimation_isGetCriticalHit()
    {
        
        playerMovement.isDodging = false;
    }

    public void OnAnimation_isStunFinished()
    {
        playerStats.isHitStun = false;
    }

    public void OnAnimation_isBlockStun()
    {
        // playerStats.isBlockStun = true;
        playerMovement.GetComponent<PlayerMovement>().isDodging = false;
    }

    public void OnAnimation_isBlockStunFinished()
    {
       // playerStats.isBlockStun = false;
    }


    #endregion

    #region Player Dodge
    public void OnAnimation_isDodging()
    {
        playerMovement.isDodging = false;
    }
    #endregion


}
