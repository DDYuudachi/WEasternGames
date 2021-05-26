//Using the state pattern from: https://www.udemy.com/course/ai-in-unity
//All subsequent states will follow this design pattern however their implementation will be unique  
using UnityEngine;
using UnityEngine.Playables;

namespace AI
{
    public class StateMachine
    {
        private State _curState;
        public PlayableAsset[] trashTalkDialogues;
        public PlayableDirector playableDirector;
        public AudioSource enemyAudioSource;

        public State _CurState
        {
            get => _curState;
            
            set
            {
                //Protection in case a state already exists
                _curState?.Exit();

                _curState = value;

                //Sets up the new state
                _curState?.Enter();
            }
        }

        public void SetTrashTalkDialogue(PlayableAsset[] trashTalkDialogue) {
            this.trashTalkDialogues = trashTalkDialogue;
        }

        public void SetPlayableDirector(PlayableDirector playableDirector) {
            this.playableDirector = playableDirector;
        }

        public void SetEnemyAudioSource(AudioSource enemyAudioSource) {
            this.enemyAudioSource = enemyAudioSource;
        }
    }
}