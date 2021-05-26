    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using AI;

public class BlockingTutorialObjective : Objective
{
    private PlayerControl playerControlScript;
    private TextMeshProUGUI tutorialTextComponent;
    private PlayerMovementV2 playerMovement;
    private FieldOfView tutorialEnemyFOV;
    public BlockingTutorialObjective(ObjectiveSystem objSys) : base(objSys)
    {
        playerControlScript = objSys.playerObject.GetComponent<PlayerControl>();
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
        playerMovement = objSys.playerObject.GetComponent<PlayerMovementV2>();
        tutorialEnemyFOV = objSys.tutorialEnemy.GetComponent<FieldOfView>();
    }

    public override void OnObjectiveStart()
    {
        // enable camera input
        objSys.cameraControls.enabled = true;

        //play fight music
        objSys.fightMusic.Play();

        // play dialogue timeline
        objSys.playableDirector.playableAsset = objSys.blockingTutorialDialogue;
        objSys.StartCoroutine(this.PlayCutscene(objSys.blockingTutorialDialogue.duration));

        objSys.playerInput.OnSkipTutorialButtonPressed += skipTutorial;
    }

    public override void OnObjectiveCompleted()
    {
        // nothing needed
    }

    public void OnBlockButtonPressed() {
        // complete the objective
        // this.ObjectiveCompleted();

        // testing no follow up cutscene

        // reset text
        tutorialTextComponent.text = "";

        // untrack block button input
        objSys.playerInput.OnRightClickPressed -= OnBlockButtonPressed;

        // resume time
        objSys.timeManager.ChangeTimescale(1);

        objSys.StartCoroutine(this.CompleteObjectiveInSeconds(2));
    }

    public void OnDialogueFinishedPlaying() {
        // prompt the tutorial

        // enable blocking
        playerControlScript.blockingActive = true;
        
        // set black background

        // stop time
        objSys.timeManager.ChangeTimescale(0);
        
        // track block button input
        objSys.playerInput.OnRightClickPressed += OnBlockButtonPressed;

        // set text
        tutorialTextComponent.color = new Color(1,1,1,1);
        tutorialTextComponent.text = "Hold the Right Mouse Button to Block";
    }

    private IEnumerator PlayCutscene(double duration) {
        objSys.playableDirector.Play();
        yield return new WaitForSeconds((float)duration);
        objSys.playableDirector.Stop();
        this.OnDialogueFinishedPlaying();
    }

    private void skipTutorial() {
        this.nextObjective = objSys.objectives[6];

         // switch off the AI
        tutorialEnemyFOV.enabled = true;

        // disable player attack, locking on and blocking
        playerControlScript.attackActive = true;
        playerControlScript.lockOnActive = true;
        playerControlScript.blockingActive = true;
        playerControlScript.sprintActive = true;

        // disable movement input
        playerMovement.enableMovement = true;

        // disable camera input
        objSys.cameraControls.enabled = true;

        objSys.StopAllCoroutines();
        objSys.playableDirector.time = 100;
        

        this.ObjectiveCompleted();
    }
}
