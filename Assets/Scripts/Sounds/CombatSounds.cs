using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSounds : SoundSystem
{
    public CombatSoundsLibrary combatSoundsLibrary;
    public GameObject diageticSoundManagerGameObject;
    public GameObject feetPosition;
    public GameObject mouthPosition;
    public GameObject swordImpactPosition;
    private DiageticSoundManager diageticSoundManager;
    // Start is called before the first frame update
    void Start()
    {
        diageticSoundManager = diageticSoundManagerGameObject.GetComponent<DiageticSoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAnimation_IsLightAttackActive()
    {
        // pick a random sound from the array
        AudioClip randomSwingSound = RandomClip(combatSoundsLibrary.lightAttackSwingSounds);

        diageticSoundManager.Add3DSound(randomSwingSound, swordImpactPosition, 1.0f); 

        //Debug.Log("Light Attack Sound");
    }

    public void OnAnimation_isGetCriticalHit()
    {
        // pick a random sound from the array
        AudioClip randomSwingSound = RandomClip(combatSoundsLibrary.swordBodySlashSounds);

        diageticSoundManager.Add3DSound(randomSwingSound, swordImpactPosition, 0.5f); 

        //Debug.Log("Light Attack Sound");
    }

    public void OnAnimation_IsHeavyAttackActive()
    {
        // pick a random sound from the array
        AudioClip randomSwingSound = RandomClip(combatSoundsLibrary.heavyAttackSwingSounds);

        diageticSoundManager.Add3DSound(randomSwingSound, swordImpactPosition, 1.0f); 
    }

    public void OnAnimation_BlockingImpact() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.swordBlockSounds);

        diageticSoundManager.Add3DSound(randomSound, swordImpactPosition.transform.position, 0.3f); 
    }

    public void OnAnimation_isBlockStun() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.swordBlockSounds);

        diageticSoundManager.Add3DSound(randomSound, swordImpactPosition.transform.position, 0.3f); 
    }

    // impact from perfect block animation start event
    public void OnAnimation_StopAttackCollision() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.perfectBlockSounds);

        diageticSoundManager.Add3DSound(randomSound, swordImpactPosition.transform.position, 0.3f); 
    }

    public void OnAnimation_HeavyLeftFootScrape() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.footScrapeSounds);

        diageticSoundManager.Add3DSound(randomSound, feetPosition, 0.1f); 
    }
    public void OnAnimation_HeavyRightFootPullBack() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.walkFootstepSounds);

        diageticSoundManager.Add3DSound(randomSound, feetPosition, 0.3f); 
    }

    public void OnAnimation_SwordHoldFirm() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.swordHoldBlockSounds);

        diageticSoundManager.Add3DSound(randomSound, swordImpactPosition, 0.3f); 
    }
    
    public void OnAnimation_WalkFootstep() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.walkFootstepSounds);

        diageticSoundManager.Add3DSound(randomSound, feetPosition, 0.5f); 
    }

    public void OnAnimation_RunFootstep() {
        // pick a random sound from the array
        AudioClip randomSound = RandomClip(combatSoundsLibrary.runFootstepSounds);

        diageticSoundManager.Add3DSound(randomSound, feetPosition, 0.5f); 
    }
}
