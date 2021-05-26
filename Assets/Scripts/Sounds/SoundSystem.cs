using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    // helper method to choose a random clip from an array
    protected AudioClip RandomClip(AudioClip[] clips) {
        System.Random random = new System.Random();

        int randomIndex;

        if (clips.Length > 1) {
            randomIndex = random.Next(0, clips.Length);
        } else {
            randomIndex = 0;
        }
        
        return clips[randomIndex];
    }
}
