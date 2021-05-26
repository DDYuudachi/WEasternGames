using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedToPlayer : MonoBehaviour
{
    public GameObject playerObject;
    public Vector3 initialWorldPosition;

    // Update is called once per frame
    void Update()
    {
        // keep the position relative to the player
        gameObject.transform.position = playerObject.transform.position + initialWorldPosition;
    }
}
