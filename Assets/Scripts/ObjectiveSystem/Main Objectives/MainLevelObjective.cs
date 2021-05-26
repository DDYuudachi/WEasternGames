using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using AI;

public class MainLevelObjective : Objective
{
    PlayerCollision playerCollision;
    PlayerStats playerStats;
    TextMeshProUGUI tutorialTextComponent;
    float gettingHitDialogueChance = 0.25f;
    private PlayerControl playerControlScript;
    private PlayerMovementV2 playerMovement;
    private FieldOfView tutorialEnemyFOV;
    
    int enemiesDefeated = 0;
    int enemyKillGoal = 3;
    public MainLevelObjective(ObjectiveSystem objSys) : base(objSys) {
        tutorialTextComponent = objSys.tutorialText.GetComponent<TextMeshProUGUI>();
        playerCollision = objSys.playerObject.GetComponent<PlayerCollision>();
        playerStats = objSys.playerObject.GetComponent<PlayerStats>();
        playerControlScript = objSys.playerObject.GetComponent<PlayerControl>();
        playerMovement = objSys.playerObject.GetComponent<PlayerMovementV2>();
        tutorialEnemyFOV = objSys.tutorialEnemy.GetComponent<FieldOfView>();
    }   

    public override void OnObjectiveCompleted()
    {
        // move characters to their positions
        objSys.playerObject.transform.position = objSys.endingCutscenePosition.position + (Vector3.forward * -5 ) + (Vector3.right * -5 );
        objSys.playerObject.transform.eulerAngles = Vector3.zero;

        objSys.tutorialEnemy.transform.position = objSys.endingCutscenePosition.position;
        objSys.tutorialEnemy.transform.eulerAngles = new Vector3(0, 190, 0);

        // disable all gameplay
        playerControlScript.enabled = false;
        playerMovement.enabled = false;
        tutorialEnemyFOV.enabled = false;

        // unregister enemy killed events
        foreach (GameObject enemy in objSys.enemies){
            enemy.GetComponent<Enemy>().OnEnemyKilled -= this.OnEnemyKilled;
            enemy.GetComponent<AIController>().enabled = false;
        }

        //transfer to ending cutscene snapshot
        objSys.endingMixerSnapshot.TransitionTo(0.5f);
        
        // unregister to sachi killed event
        playerStats.OnPlayerDeath -= OnPlayerDead;

        // unregister to sachi getting hit events
        playerCollision.OnPlayerHurt -= OnSachiGetHit;

        // play ending cutscene
        objSys.StopAllCoroutines();
        objSys.playableDirector.playableAsset = objSys.endingCutscene;
        objSys.playableDirector.Play();

    }

    public override void OnObjectiveStart()
    {   
        // introduce other buttons
        objSys.StartCoroutine(OtherButtonsTutorial());

        // display objective for a brief moment
        objSys.StartCoroutine(ShowObjectives());

        // register to sachi killed event
        playerStats.OnPlayerDeath += OnPlayerDead;

        // register to sachi getting hit events
        playerCollision.OnPlayerHurt += OnSachiGetHit;
    }

    private void OnSachiGetHit() {
        // check if sachi is dead, if dead then play the death dialogue
        if (playerStats.isDeath) { return; }

        // random chance of triggering the dialogues
        if (Random.Range(0f, 1f) > this.gettingHitDialogueChance) { return; }

        // check if playable is still playing
        if (objSys.playableDirector.state == PlayState.Playing) { return; }

        // choose a random hurt dialogue
        if (objSys.SachiHurtDialogues.Length == 0) { return; }
        int random = Random.Range(0,objSys.SachiHurtDialogues.Length);
        objSys.StartCoroutine(PlayCutscene(objSys.SachiHurtDialogues[random]));
    }

    public void OnEnemyKilled() {
        // increment number of defeated enemies
        enemiesDefeated++;

        // show updated objective display 
        objSys.defeatEnemiesText.text = "Defeat Enemies (" + enemiesDefeated + "/" + enemyKillGoal + ")";
        objSys.StartCoroutine(ShowObjectives());

        // check is it equals to number of enemies
        if (enemiesDefeated >= enemyKillGoal) {
            // complete the objective
            this.ObjectiveCompleted();
        }
    }

    public void OnPlayerDead() {
        // switch off all AI
        foreach (GameObject enemy in objSys.enemies) {
            enemy.GetComponent<AIController>().enabled = false;
        }

        // play the death dialogue
        objSys.StopAllCoroutines();
        objSys.StartCoroutine(PlayCutscene(objSys.SachiDeathDialogue));

        // unregister to sachi killed event
        playerStats.OnPlayerDeath -= OnPlayerDead;

        // unregister to sachi getting hit events
        playerCollision.OnPlayerHurt -= OnSachiGetHit;
    }

    private IEnumerator PlayCutscene(PlayableAsset cutscene) {
        float duration = (float)cutscene.duration;
        objSys.playableDirector.playableAsset = cutscene;
        objSys.playableDirector.Play();
        yield return new WaitForSeconds(duration);
        objSys.playableDirector.Stop();
    }

    private IEnumerator OtherButtonsTutorial() {
        yield return new WaitForSeconds(5);
        tutorialTextComponent.text = "Press Q to cycle through enemies";
        yield return new WaitForSeconds(5);
        tutorialTextComponent.text = "";
        yield return new WaitForSeconds(5);
        tutorialTextComponent.text = "Tap Shift to dodge attacks";
        yield return new WaitForSeconds(5);
        tutorialTextComponent.text = "";
        yield return new WaitForSeconds(5);
        tutorialTextComponent.text = "Hold Left click to do a heavy attack";
        yield return new WaitForSeconds(5);
        tutorialTextComponent.text = "";
    }

    private IEnumerator ShowObjectives() {
        objSys.objectiveHeadingText.SetActive(true);
        yield return new WaitForSeconds(5);
        objSys.objectiveHeadingText.SetActive(false);
    }
}
