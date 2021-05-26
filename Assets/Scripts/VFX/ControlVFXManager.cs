using UnityEngine;
using UnityEngine.VFX;

public class ControlVFXManager : MonoBehaviour
{
    public float FPS;
    // Start is called before the first frame update
    void Start()
    {
        VFXManager.fixedTimeStep = 1/FPS;
    }
}
