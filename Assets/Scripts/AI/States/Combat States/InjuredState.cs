using AI;
using AI.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Animation;
using Utilities;

public class InjuredState : State
{
    private AnimationAction _injuredAction;
    
    
    private bool _complete = false;
    private float _animTime;
    private EnemyAction _enemyAction;
    private Animator _playerAnimator;
    private AIController _aiController;
    private bool _played;
    
    private static readonly int IsInjured = Animator.StringToHash("isInjured");

    public InjuredState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _animator = _go.GetComponent<Animator>();
        _enemyAction = (EnemyAction) _attributes.Find(x => x.GetType() == typeof(EnemyAction));
        _aiController = (AIController) _attributes.Find(x => x.GetType() == typeof(AIController));

        _playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        
        
        _enemyAction.action = EnemyAction.EnemyActionType.Injured;
        
        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (!clip.name.Contains("takeDMG")) continue;
            
            _injuredAction = new AnimationAction(clip);
            _animTime = _injuredAction.AnimationClipLength * 2;
        }

        _played = false;

        /*
        if (!_playerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("HT"))
            _animTime /= 4;*/
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(!_complete)
            PlayInjured();

        if (_complete)
        {
            Debug.Log("Complete");
            _aiController.injuryCooldown = 1.5f;
            _sm._CurState = new AttackingState(_go, _sm, _attributes, _animator);
        }
            
        //_sm._CurState = new AttackingState(_go, _sm, _attributes, _animator);
    }

    private void PlayInjured()
    {
        if (!_played)
        {
            _played = true;
            _animator.SetTrigger(IsInjured);
        }

    _animTime -= Time.fixedDeltaTime;

        if (_animTime <= 0f)
            _complete = true;
    }
}
