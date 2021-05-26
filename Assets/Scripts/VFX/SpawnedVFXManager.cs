using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnedVFXManager : MonoBehaviour
{
    private LinkedList<GameObject> activeVFXList;
    private Queue<GameObject> finishedVFXQueue;
    // Start is called before the first frame update
    void Start()
    {
        activeVFXList = new LinkedList<GameObject>();
        finishedVFXQueue = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if effect is finished playing
        foreach(GameObject obj in activeVFXList) {
            VisualEffect vfxComponent = obj.GetComponent<VisualEffect>();
            if (vfxComponent.aliveParticleCount == 0) { // check if VFX is finished playing
                finishedVFXQueue.Enqueue(obj); // add to queue for deletion, modification mid iteration is not allowed
            }
        }

        // remove from list, delete finished VFX objects, use the queue
        while (finishedVFXQueue.Count > 0) {
            GameObject finished = finishedVFXQueue.Dequeue(); // dequeue to get reference to finished VFX object
            activeVFXList.Remove(finished);  //remove from the list, so won't cause any reference issues
            Destroy(finished); // destroy game object, deleting from list does not destroy game object, still exists in game world
        }
    }

    public void AddVFX(GameObject VFXObject) {
        this.activeVFXList.AddLast(VFXObject);
    }
}
