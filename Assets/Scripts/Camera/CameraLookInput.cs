using UnityEngine;
using Cinemachine;
 
/// <summary>
/// A simple script that allows the Cinemachine virtual camera to look around. this script must be added to a CM virtual camera
/// </summary>
[RequireComponent(typeof(CinemachineVirtualCameraBase))]
public class CameraLookInput : MonoBehaviour
{
    public float Speed = 5f;
    public float PitchMax = 70f;
    public float PitchMin = 45f;
 
    private CinemachineVirtualCamera cam;
    private Transform lookAt;
 
    private float _currentX, _currentY;
 
    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
 
        lookAt = new GameObject("LookAtTarget").transform;
        lookAt.position = transform.position;
        lookAt.position += transform.forward * 10f;
 
        cam.LookAt = lookAt;
    }
 
    private void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
 
        _currentX = Mathf.Clamp(_currentX + y * Speed, PitchMin, PitchMax);
        _currentY += x * Speed;
 
        Vector3 dir = Vector3.forward * 10f;
 
        Vector3 input = new Vector3(-_currentX, _currentY);
        Quaternion r = Quaternion.Euler(input);
 
        Vector3 freeLook = r * dir;
        Vector3 pos = transform.position + freeLook;
        lookAt.position = pos;
    }
}