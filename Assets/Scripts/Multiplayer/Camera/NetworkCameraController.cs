using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using UnityEngine;

public class NetworkCameraController : NetworkBehaviour
{
    [SerializeField] private Vector2 _maxFollowOffset = new Vector2(-1, 6f);
    [SerializeField] private Vector2 _cameraVelocity = new Vector2(4f, 0.25f);
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CinemachineFreeLook _freeLookCamera = null;
    private CinemachineTransposer _transposer;

    public override void OnStartAuthority()
    {
        _freeLookCamera.gameObject.SetActive(true);

        enabled = true;
    }

    
}