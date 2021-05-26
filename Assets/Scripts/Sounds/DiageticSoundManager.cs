using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DiageticSoundManager : MonoBehaviour
{
    private LinkedList<GameObject> audioSourceList;
    private Queue<GameObject> finishedAudioSourceQueue;

    public AudioMixerGroup audioMixerGroup;
    // Start is called before the first frame update
    void Start()
    {
        audioSourceList = new LinkedList<GameObject>();
        finishedAudioSourceQueue = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if finished playing then remove from list.. is it destroyed in memory?.. maybe garbage collector handles that

        foreach(GameObject source in audioSourceList) {
            AudioSource audioSource = source.GetComponent<AudioSource>();
            if (!audioSource.isPlaying) { // check if audio is finished playing
                finishedAudioSourceQueue.Enqueue(source); // add to queue for deletion, modification mid iteration is not allowed
            }
        }

        // remove from list, delete sound objects, use the queue

        while (finishedAudioSourceQueue.Count > 0) {
            GameObject finished = finishedAudioSourceQueue.Dequeue(); // dequeue to get reference to finished audio object
            audioSourceList.Remove(finished);  //remove from the list, so won't cause any reference issues
            Destroy(finished); // destroy game object, deleting from list does not destroy game object, still exists in game world
        }
    }

    // add sound that is attached to a gameobject. e.g footsteps, dialogue
    public void Add3DSound(AudioClip audioClip, GameObject parentObject, float volume) {
        GameObject newSoundObject = new GameObject(); // create a game object, cant directly create a sound source

        newSoundObject.transform.parent = parentObject.transform;

        newSoundObject.transform.localPosition = new Vector3(0,0,0);

        AudioSource newSource = newSoundObject.AddComponent<AudioSource>(); // add a sound source component to object

        audioSourceList.AddLast(newSoundObject); // put into the queue

        SetAudioSourceParameters(newSource, audioClip, volume, 1.0f); // set parameters of sound source

        newSource.Play(); // immediately start playing sound
    }

    // free floating sound variant
    public void Add3DSound(AudioClip audioClip, Vector3 worldPosition, float volume) {
        GameObject newSoundObject = new GameObject(); // create a game object, cant directly create a sound source

        newSoundObject.transform.position = worldPosition; // position the sound where specified

        AudioSource newSource = newSoundObject.AddComponent<AudioSource>(); // add a sound source component to object

        audioSourceList.AddLast(newSoundObject); // put into the queue

        SetAudioSourceParameters(newSource, audioClip, volume, 1.0f); // set parameters of sound source

        newSource.Play(); // immediately start playing sound
    }

    public void AddDelayed3DSoundInRandomPositionAroundPlayer(AudioClip audioClip, GameObject playerObject, float volume, float delay) {
        GameObject newSoundObject = new GameObject(); // create a game object, cant directly create a sound source

        // generate a random direction on the X and Z, can be much better than this
        Vector2 random2D = Random.insideUnitCircle.normalized;
        Vector3 randomDirection = new Vector3(
            random2D.x,
            0,
            random2D.y
        );
        // multiply along the direction depending on inner and outer radius
        Vector3 randomPosition = Random.Range(10.0f, 15.0f) * randomDirection.normalized;

        AttachedToPlayer attachedToPlayerComponent = newSoundObject.AddComponent<AttachedToPlayer>(); // add the component that will keep the position relative to the player
        AudioSource newSource = newSoundObject.AddComponent<AudioSource>(); // add a sound source component to object
        newSource.outputAudioMixerGroup = audioMixerGroup;

        attachedToPlayerComponent.playerObject = playerObject;
        attachedToPlayerComponent.initialWorldPosition = randomPosition;

        audioSourceList.AddLast(newSoundObject); // put into the queue

        SetAudioSourceParameters(newSource, audioClip, volume, 1.0f); // set parameters of sound source

        newSource.PlayDelayed(delay); // immediately start playing sound
    }

    public void SetAudioSourceParameters(AudioSource source, AudioClip clip, float volume, float spatialBlend) {
        source.volume = volume; // adjust volume
        source.spatialBlend = spatialBlend; // set 3D Sound
        source.clip = clip; // assign audio clip
        source.dopplerLevel = 0; // always turn off doppler?
    }
}
