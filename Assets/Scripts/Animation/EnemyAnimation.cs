﻿using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using Utilities;

public class EnemyAnimation : MonoBehaviour, IAIAttribute
{
    public Animator _anim;
    private EnemyAction enemyAction;
    public GameObject player;
    public GameObject enemy;
    public Collider enemyWeapon;
    public SwordEffectSpawner weaponClashSpawner;

    void Start()
    {
        _anim = GetComponent<Animator>();
        enemyAction = GetComponent<EnemyAction>();
    }

    void FixedUpdate()
    {
        initialiseAnimatorBool();
    }

    void initialiseAnimatorBool()
    {
        //_anim.SetBool("isAttacking", enemyWeapon.isTrigger);
        //_anim.SetBool("isKeepBlocking", enemyAction.isKeepBlocking);
        //_anim.SetBool("isPerfectBlock", enemyAction.isPerfectBlock);
        //_anim.SetBool("isInPerfectBlockOnly", enemyAction.isInPerfectBlockOnly);
    }

    #region Enemy Attack Logic
    public void OnAnimation_IsHeavyAttackActive()
    {
        enemyWeapon.isTrigger = false;
    }

    public void OnAnimation_IsHeavyAttackDeactive()
    {
        enemyWeapon.isTrigger = true;
    }

    public void OnAnimation_IsLightAttackActive()
    {
        enemyWeapon.isTrigger = false;
    }

    public void OnAnimation_IsLightAttackDeactive()
    {
        enemyWeapon.isTrigger = true;
    }

    public void OnAnimation_StopAttackCollision()
    {
        weaponClashSpawner.SpawnBigSwordClash();
        enemyWeapon.isTrigger = true;
    }
    #endregion

    #region Enemy Block Logic
    public void OnAnimation_isBlockStart()
    {
        enemyAction.isKeepBlocking = true;
    }

    public void OnAnimation_BlockStart()
    {
        enemyAction.isKeepBlocking = true;
    }

    public void OnAnimation_isPerfectBlock()
    {
        enemyAction.isPerfectBlock = true;
    }

    public void OnAnimation_isPerfectBlockEnd()
    {
        enemyAction.isPerfectBlock = false;
    }
    #endregion

    #region Enemy Get Hurt Logic
    public void OnAnimation_isGetCriticalHit()
    {
    }
    #endregion

    public void OnAnimation_isBlockStun()
    {

    }

    public void OnAnimation_isStunFinished()
    {

    }

    public void OnAnimation_isBlockStunFinished()
    {

    }
}
