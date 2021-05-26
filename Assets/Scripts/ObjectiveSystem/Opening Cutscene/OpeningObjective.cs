using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using AI;

public class OpeningObjective : Objective
{
    private PlayerControl playerControlScript;
    private TextMeshProUGUI skipText;
    private Image arrowsImage;
    private Image spacebarIcon;
    private PlayerMovementV2 playerMovement;
    private FieldOfView tutorialEnemyFOV;
    private float skipTimer = 0f;
    private float skipDuration = 2f;
    public OpeningObjective(ObjectiveSystem objSys) : base(objSys) {
        playerControlScript = objSys.playerObject.GetComponent<PlayerControl>();
        playerMovement = objSys.playerObject.GetComponent<PlayerMovementV2>();
        tutorialEnemyFOV = objSys.tutorialEnemy.GetComponent<FieldOfView>();
        tutorialEnemyFOV = objSys.tutorialEnemy.GetComponent<FieldOfView>();
        Image[] imageComponents = objSys.skipCutsceneUI.GetComponentsInChildren<Image>();
        this.arrowsImage = imageComponents[0];
        this.spacebarIcon = imageComponents[1];
        this.skipText = objSys.skipCutsceneUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void OnObjectiveStart()
    {
        // switch off the AI
        tutorialEnemyFOV.enabled = false;

        // disable player attack, locking on and blocking
        playerControlScript.attackActive = false;
        playerControlScript.lockOnActive = false;
        playerControlScript.blockingActive = false;
        playerControlScript.sprintActive = false;

        // disable movement input
        playerMovement.enableMovement = false;

        // disable camera input
        objSys.cameraControls.enabled = false;

        // play opening cutscene
        objSys.StartCoroutine(PlayCutscene(objSys.flybyCutscene));

        // use play the cutscene audio for x seconds
        objSys.StartCoroutine(PlayIntroAudio(55));

        objSys.playerInput.OnSkipButtonHold += skipButtonHold;
        objSys.playerInput.OnSkipButtonNotHeld += skipButtonNotHeld;

        // skip button UI
        this.skipText.color = new Color(1,1,1,1);
        this.arrowsImage.color = new Color(1,1,1,1);
        this.spacebarIcon.color = new Color(1,1,1,1);

    }

    public override void OnObjectiveCompleted()
    {
        objSys.playerInput.OnSkipButtonHold -= skipButtonHold;
        objSys.playerInput.OnSkipButtonNotHeld -= skipButtonNotHeld;
        this.skipText.color = new Color(1,1,1,0);
        this.arrowsImage.color = new Color(1,1,1,0);
        this.spacebarIcon.color = new Color(1,1,1,0);
    }

    public void OnDialogueFinishedPlaying() {
        // go to tutorial
        this.ObjectiveCompleted();
    }

    private void skipButtonHold() {
        this.skipTimer = Mathf.Clamp(this.skipTimer + Time.deltaTime, 0, this.skipDuration);

        if (this.skipTimer >= this.skipDuration) {
            objSys.StopAllCoroutines();
            objSys.StartCoroutine(skipCutscene());
        }

        this.arrowsImage.fillAmount = this.skipTimer/this.skipDuration;
    }

    private void skipButtonNotHeld() {
        this.skipTimer = Mathf.Clamp(this.skipTimer - Time.deltaTime, 0, this.skipDuration);
        this.arrowsImage.fillAmount = this.skipTimer/this.skipDuration;
    }
    
    private IEnumerator skipCutscene() {
        // skip button UI
        this.skipText.color = new Color(1,1,1,0);
        this.arrowsImage.color = new Color(1,1,1,0);
        this.spacebarIcon.color = new Color(1,1,1,0);
        objSys.playerInput.OnSkipButtonHold -= skipButtonHold;
        objSys.playerInput.OnSkipButtonNotHeld -= skipButtonNotHeld;
        objSys.fightMixerSnapshot.TransitionTo(1.5f);
        objSys.playableDirector.time = objSys.playableDirector.duration - 1.5;
        yield return new WaitForSeconds(1.5f);
        objSys.playableDirector.Stop();
        this.OnDialogueFinishedPlaying();
    }

    private IEnumerator PlayCutscene(PlayableAsset cutscene) {
        float duration = (float)cutscene.duration;
        objSys.playableDirector.playableAsset = cutscene;
        objSys.playableDirector.Play();
        yield return new WaitForSeconds(duration);
        objSys.playableDirector.Stop();
        this.OnDialogueFinishedPlaying();
    }

    private IEnumerator PlayIntroAudio(float duration) {
        yield return new WaitForSeconds(duration);
        objSys.fightMixerSnapshot.TransitionTo(1.5f);
    }
}
