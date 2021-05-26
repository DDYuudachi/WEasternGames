﻿using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
using UnityEngine;

public class CombatWalk : State
{
    private Transform _player;
    
    private float _moveSpeed;
    private bool _forward;
    
    #region Animation Triggers
    
    private float _zVel;
    private int _zVelHash;

    #endregion
    

    public CombatWalk(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator, bool forward) : base(go, sm, attributes, animator)
    {
        _forward = forward;
    }

    public override void Enter()
    {
        base.Enter();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _moveSpeed = 2f;
        _zVelHash = Animator.StringToHash("enemyVelZ");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float distanceToPlayer = Vector3.Distance(_go.transform.position, _player.position);


        if (_forward)
        {
            _zVel = 1;
            //_go.transform.LookAt(_player.position);
            //https://forum.unity.com/threads/smooth-look-at.26141/  smooth rotate and dont rotate the Y axis
            Vector3 lookPosition = _player.position - _go.transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            _go.transform.rotation = Quaternion.Slerp(_go.transform.rotation, rotation, Time.deltaTime * Enemy.EnemyRotationSpeed);
            _go.transform.position += _go.transform.forward * (_moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _zVel = -1;
            //_go.transform.LookAt(_player.position);
            //https://forum.unity.com/threads/smooth-look-at.26141/  smooth rotate and dont rotate the Y axis
            Vector3 lookPosition = _player.position - _go.transform.position;
            lookPosition.y = 0;
            var rotation = Quaternion.LookRotation(lookPosition);
            _go.transform.rotation = Quaternion.Slerp(_go.transform.rotation, rotation, Time.deltaTime * Enemy.EnemyRotationSpeed);
            _go.transform.position -= _go.transform.forward * ((_moveSpeed + 2) * Time.fixedDeltaTime);
        }

        _animator.SetFloat(_zVelHash, _zVel);

        //The AI is walking toward the player so it will then enter combat again this will also trigger if the player
        //runs after the AI and catches up to them
        if (distanceToPlayer < 1.6 && _forward)
        {
            _zVel = 0;
            _animator.SetFloat(_zVelHash, _zVel);
            _sm._CurState = new AttackingState(_go, _sm, _attributes, _animator);
        }

        //The AI is walking away from the player to enter an evasive state
        if (distanceToPlayer >= 10.0f && !_forward)
        {
            _zVel = 0;
            _animator.SetFloat(_zVelHash, _zVel);
            _sm._CurState = new CombatWalk(_go, _sm, _attributes, _animator, true);
        }
        
    }
}
