using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Audio;
using Cinemachine;
using TMPro;

public class ObjectiveSystem : MonoBehaviour
{
    // any unity scene objects that needs to bee referenced is got from here
    public PlayerInput playerInput;
    public TimeManager timeManager;
    public PlayableDirector playableDirector;
    public GameObject playerObject;
    public CinemachineBrain cameraControls;
    public GameObject tutorialText;
    public GameObject objectiveHeadingText;
    public TextMeshProUGUI defeatEnemiesText;
    public GameObject skipCutsceneUI;
    public GameObject tutorialEnemyWeapon;
    public GameObject tutorialEnemy;
    public PlayableAsset flybyCutscene;
    public PlayableAsset blockingTutorialDialogue;
    public PlayableAsset lockOnTutorialDialogue;
    public PlayableAsset bringItOnDialogue;
    public PlayableAsset enemyShoutingDialogue;
    public PlayableAsset AfterAttackTutorialDialogue;
    public PlayableAsset SachiDeathDialogue;
    public PlayableAsset endingCutscene;

    public PlayableAsset[] SachiHurtDialogues;

    public GameObject[] enemies;

    public AudioMixerSnapshot fightMixerSnapshot;
    public AudioMixerSnapshot endingMixerSnapshot;
    public AudioSource fightMusic;

    public Transform endingCutscenePosition;

    // objectives
    public Objective[] objectives;
    void Start() {
        // initialize number of objectives
        this.objectives = new Objective[7];

        // initialize all objectives, assigning the references to unity scene objects
        this.objectives[0] = new OpeningObjective(this);
        this.objectives[1] = new BlockingTutorialObjective(this);
        this.objectives[2] = new LockOnTutorialObjective(this);
        this.objectives[3] = new MovementTutorialObjective(this);
        this.objectives[4] = new PerfectBlockTutorialObjective(this);
        this.objectives[5] = new AttackingTutorialObjective(this);
        this.objectives[6] = new MainLevelObjective(this);

        // set the order of the objectives
        this.objectives[0].SetNextObjective(this.objectives[1]);
        this.objectives[1].SetNextObjective(this.objectives[2]);
        this.objectives[2].SetNextObjective(this.objectives[3]);
        this.objectives[3].SetNextObjective(this.objectives[4]);
        this.objectives[4].SetNextObjective(this.objectives[5]);
        this.objectives[5].SetNextObjective(this.objectives[6]);

        // start the first objective
        this.objectives[0]?.ObjectiveStart();
        // this.objectives[6]?.ObjectiveStart();

        // enemies
        foreach (GameObject enemy in enemies){
            enemy.GetComponent<Enemy>().OnEnemyKilled += ((MainLevelObjective)this.objectives[6]).OnEnemyKilled;
        }
    }
}
