using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SwordEffectSpawner : MonoBehaviour
{
    public VisualEffect visualEffectObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSwordClash() {
        visualEffectObject.SendEvent("OnSwordClash");
    }
    public void SpawnBigSwordClash() {
        visualEffectObject.SendEvent("OnPerfectBlock");
    }
}
