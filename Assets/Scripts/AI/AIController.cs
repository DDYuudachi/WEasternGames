using System;
using System.Collections.Generic;
using AI;
using AI.States;
using UnityEngine;
using Utilities;
using UnityEngine.Playables;

public class AIController : MonoBehaviour, IAIAttribute
{
    private StateMachine _sm;
    private EnemyAction _enemyAction;
    private Enemy _enemy;
    private Animator _anim;
    private List<IAIAttribute> _aiAttributes;
    private FieldOfView _fieldOfView;

    private int _attacked;
    public float injuryCooldown = 0;
    public int id;
    public bool done = false;

    public bool HasAttackFlag { get; set; }
    public PlayableAsset[] trashTalkDialogue;
    public PlayableDirector playableDirector;
    public PlayerAction playerAction;


    public enum InitialAIState
    {
        Idle,
        Patrol,
    }

    public InitialAIState initialAIState;

    private void Awake()
    {
        HasAttackFlag = false;
        _sm = new StateMachine();
        _enemyAction = GetComponent<EnemyAction>();
        _anim = GetComponent<Animator>();
        _attacked = 0;
        _sm.SetTrashTalkDialogue(trashTalkDialogue);
        _sm.SetPlayableDirector(playableDirector);
        _sm.SetEnemyAudioSource(GetComponentInChildren<AudioSource>());
        //AIManager.current.OnAttackStateChangeReq += OnAttackStateChange;
        _aiAttributes = new List<IAIAttribute>();
        _enemy = GetComponent<Enemy>();
        _fieldOfView = GetComponent<FieldOfView>();

        //AI Attributes to be passed to all of the states so that GetComponent needn't be called in every state once it's entered 
        _aiAttributes.Add(this);
        _aiAttributes.Add(_fieldOfView);
        _aiAttributes.Add(GetComponent<EnemyAnimation>());
        _aiAttributes.Add(_enemy);
        _aiAttributes.Add(GetComponent<EnemyCollision>());
        _aiAttributes.Add(_enemyAction);

        foreach (IAIAttribute attribute in _aiAttributes)
        {
            //Debug.Log(attribute.GetType());
        }
    }

    private void Start()
    {
        //Debug.Log("AIController " + id + " is printing " + AIManager.current);
        _sm._CurState = new IdleState(gameObject, _sm, _aiAttributes, _anim);
    }

    // Update is called once per frame
    void Update()
    {
        _sm._CurState.Update();
    }

    private void FixedUpdate()
    {

        float distanceToPlayer = Vector3.Distance(gameObject.transform.position, _fieldOfView.Player.transform.position);

        _sm._CurState.FixedUpdate();

        if (injuryCooldown != 0)
            injuryCooldown -= Time.fixedDeltaTime;

        if (injuryCooldown <= 0f && _enemyAction.action != EnemyAction.EnemyActionType.HeavyAttack &&
            _enemyAction.action == EnemyAction.EnemyActionType.EnterInjured)
            _sm._CurState = new InjuredState(gameObject, _sm, _aiAttributes, _anim);


        if (_attacked == 5)
            _sm._CurState = new BlockingState(gameObject, _sm, _aiAttributes, _anim);

        if (_enemyAction.action != EnemyAction.EnemyActionType.HeavyAttack &&
            playerAction.isPlayerAttacking && !done && _enemy.stamina > 0 && distanceToPlayer <= 2)
        {
            done = true;
            _sm._CurState = new QuickBlock(gameObject, _sm, _aiAttributes, _anim);
        }
    }

    public void AttackStateChange(AIController controller)
    {
        if (controller == this)
            _sm._CurState = new AttackingState(gameObject, _sm, _aiAttributes, _anim);
    }

    public void EvasiveStateChange(AIController controller)
    {
        if (controller == this)
            _sm._CurState = new EvasiveState(gameObject, _sm, _aiAttributes, _anim);
    }

    public void EvasionEnvironmentCollided()
    {
        _anim.SetFloat("EnemyX", 0);
        _sm._CurState = new CombatWalk(gameObject, _sm, _aiAttributes, _anim, true);
    }
}
