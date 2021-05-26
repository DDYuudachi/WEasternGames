using UnityEngine;
[ExecuteInEditMode]
public class EnableDepthTexture : MonoBehaviour
{
    void Start()
    {
     gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}
