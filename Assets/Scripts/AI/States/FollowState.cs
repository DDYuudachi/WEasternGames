using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace AI.States
{
    public class FollowState : State
    {
        private float _moveSpeed;

        private EnemyAction _enemyAction;
        private FieldOfView _fieldOfView;

        #region Animation Triggers

        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private float _zVel;
        private int _zVelHash;

        private int _sequence;
        private float _timeRemaining;

        #endregion

        public FollowState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator, int sequence = 0, float timeRemaining = 0)
            : base(go, sm, attributes, animator)
        {
            _sequence = sequence;
            timeRemaining = timeRemaining;
            //AIManager.current.OnAttackStateChangeReq += OnAttackStateChange;
        }

        public override void Enter()
        {
            base.Enter();
            _fieldOfView = (FieldOfView)_attributes.Find(x => x.GetType() == typeof(FieldOfView));
            _enemyAction = (EnemyAction)_attributes.Find(x => x.GetType() == typeof(EnemyAction));

            _enemyAction.action = EnemyAction.EnemyActionType.Follow;
            _moveSpeed = 8f;
            _zVelHash = Animator.StringToHash("enemyVelZ");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float distanceToPlayer = Vector3.Distance(_go.transform.position, _fieldOfView.Player.transform.position);


            if (_fieldOfView.Player != null && distanceToPlayer >= 1.5)
            {
                _zVel = 2;
                //_go.transform.LookAt(_fieldOfView.Player.transform.position);
                //https://forum.unity.com/threads/smooth-look-at.26141/  smooth rotate and dont rotate the Y axis
                Vector3 lookPosition = _fieldOfView.Player.transform.position - _go.transform.position;
                lookPosition.y = 0;
                var rotation = Quaternion.LookRotation(lookPosition);
                _go.transform.rotation = Quaternion.Slerp(_go.transform.rotation, rotation, Time.deltaTime * Enemy.EnemyRotationSpeed);
                _go.transform.position += _go.transform.forward * _moveSpeed * Time.deltaTime;
                _animator.SetFloat(_zVelHash, _zVel);
            }

            if (distanceToPlayer <= 1.5)
            {
                _zVel = 0;
                _animator.SetFloat(_zVelHash, _zVel);
                //_sm._CurState = new AttackingState(_go, _sm, _attributes, _animator, _sequence, _timeRemaining);
                AIManager.Instance.RequestsAttackState(_go);
            }
        }

        //private void OnAttackStateChange()
        //{
        //     Debug.Log("Event Triggered for changing to Attacking State");
        //}
    }
}