using System;
using AI;
using UnityEngine;

namespace UnityTemplateProjects.AI
{
    public class DemoClassWithState : MonoBehaviour
    {
        private StateMachine _sm;

        private void Awake()
        {
            //_sm = new StateMachine();
            //_sm._CurState = new ChasingState(gameObject, _sm);
        }

        private void Update()
        {
            _sm._CurState.Update();
        }

        private void FixedUpdate()
        {
            _sm._CurState.FixedUpdate();
        }
    }
}