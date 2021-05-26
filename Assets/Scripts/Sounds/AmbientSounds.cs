using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : SoundSystem
{
    [Range(0, 10)]
    public int birdsDensity;
    public float spawnInterval;
    public GameObject playerObject;
    public AnimalSoundsLibrary animalSoundsLibrary; 
    public DiageticSoundManager diageticSoundManager;
    
    private Metronome metronome;

    // Start is called before the first frame update
    void Start()
    {
        metronome = new Metronome(spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        metronome.AddTimeToStopwatch(Time.deltaTime);

        if (metronome.Triggered()) {
            // spawn sounds based on density, delay play times randomly between the intervals
            for (int i = 0; i < birdsDensity; i++) {
                // generate a random delay, values in between the interval
                float delay = Random.Range(0.0f, spawnInterval);
                // add sound in a random position around the player 
                diageticSoundManager.AddDelayed3DSoundInRandomPositionAroundPlayer(RandomClip(animalSoundsLibrary.birdSounds), playerObject, 0.5f, delay);
            }
        }
    }
}
