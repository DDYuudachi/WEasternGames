using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private enum EnemyState
    {
        Idle, 
        Tracking, 
        ReturnToDefaultPosition,
        Combat
    }

    [SerializeField]
    private EnemyState _currentState;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == EnemyState.Idle)
        {
            IdleState();
        }
        else if (_currentState == EnemyState.Tracking)
        {
            Tracking();
        }
        else if (_currentState == EnemyState.Combat)
        {
            Combat();
        }
        else if (_currentState == EnemyState.ReturnToDefaultPosition)
        {
            ReturnToDefaultPosition();
        }
    }
    
    void IdleState(){}
    void Tracking(){}
    void Combat(){}
    void ReturnToDefaultPosition(){}
}
