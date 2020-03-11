using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private CinemachineVirtualCamera _vCam;
    private CinemachineConfiner _confiner;

    public static CameraController Instance { get; private set; }

    private void Awake() {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        _vCam = GetComponent<CinemachineVirtualCamera>();
        _confiner = GetComponent<CinemachineConfiner>();
    }
    private void MoveRoom(GameObject CameraBounds) {

    }

    public void DisableConfines() {
        _confiner.enabled = false;
    }

    public void EnableConfines(Collider2D newBounds) {
        _confiner.m_BoundingShape2D = newBounds;
        _confiner.InvalidatePathCache();
        _confiner.enabled = true;
    }


}
