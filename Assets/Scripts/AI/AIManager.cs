using System;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class AIManager : MonoBehaviour
    {
        private static AIManager _instance;
        public static AIManager Instance { get { return _instance; } }


        private GameObject[] _enemies;
        private IDictionary<GameObject, AIController> _enemyControllers;


        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }


        private void Start()
        {
            _enemies = GameObject.FindGameObjectsWithTag("Enemy");
            _enemyControllers = new Dictionary<GameObject, AIController>();

            foreach (GameObject enemy in _enemies)
            {
                _enemyControllers.Add(enemy, enemy.GetComponent<AIController>());
            }
        }

        public void RequestsAttackState(GameObject reqEnemy)
        {
            Debug.Log("Request Attack State");

            AIController enemyController = _enemyControllers[reqEnemy];

            bool flagTaken = false;

            foreach (KeyValuePair<GameObject, AIController> item in _enemyControllers)
            {
                //If even one AI Controller has the flag then stop the others from grabbing it
                if (item.Value.HasAttackFlag)
                {
                    flagTaken = true;
                }

            }

            //If any AIController has the flag then don't continue
            if (flagTaken)
            {
                Debug.Log("Flag Was Taken");
                enemyController.EvasiveStateChange(enemyController);
            }
            else
            {
                Debug.Log("Flag Free");
                if (enemyController != null && !enemyController.HasAttackFlag)
                {
                    Debug.Log("Make Attack");
                    enemyController.HasAttackFlag = true;
                    enemyController.AttackStateChange(enemyController);
                }
            }
        }


        /*
        public void CheckForAttack()
        {
            bool oneAttacking = false;
            int low = 100;
            int high = 0 ;
            
            foreach (var kvp in _enemyControllers)
            {
                if (kvp.Value.id > high)
                    high = kvp.Value.id;

                if (kvp.Value.id < low)
                    low = kvp.Value.id;
                
                if (kvp.Value.HasAttackFlag)
                    oneAttacking = true;
            }

            if (!oneAttacking)
            {
                int random = Random.Range(low, high);

                AIController controllerToChange = null;

                foreach (var kvp in _enemyControllers)
                {
                    if (kvp.Value.id == random)
                    {
                        controllerToChange = kvp.Value;
                    }
                }
                
                if(controllerToChange != null)
                    controllerToChange.AttackStateChange(controllerToChange);
            }
        }*/
    }
}