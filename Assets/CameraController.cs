using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera CinemachineVirtualCamera;

    Transform player;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        CinemachineVirtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineVirtualCamera.Follow = player;
    }

   
}
