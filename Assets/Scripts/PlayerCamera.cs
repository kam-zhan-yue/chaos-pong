using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCamera : NetworkBehaviour
{
    Camera _mainCam;

    void Awake()
    {
        _mainCam = Camera.main;
    }

    public override void OnStartLocalPlayer()
    {
        // if (_mainCam != null)
        // {
        //     Transform mainCamTransform;
        //     (mainCamTransform = _mainCam.transform).SetParent(transform);
        //     mainCamTransform.localPosition = new Vector3(0f, 6f, -11f);
        //     mainCamTransform.localEulerAngles = new Vector3(25f, 0f, 0f);
        // }
        // else
        //     Debug.LogWarning("PlayerCamera: Could not find a camera in scene with 'MainCamera' tag.");
    }

    public override void OnStopLocalPlayer()
    {
        // if (_mainCam != null)
        // {
        //     _mainCam.transform.SetParent(null);
        //     SceneManager.MoveGameObjectToScene(_mainCam.gameObject, SceneManager.GetActiveScene());
        //     _mainCam.orthographic = true;
        //     _mainCam.orthographicSize = 15f;
        //     Transform mainCamTransform = _mainCam.transform;
        //     mainCamTransform.localPosition = new Vector3(0f, 70f, 0f);
        //     mainCamTransform.localEulerAngles = new Vector3(90f, 0f, 0f);
        // }
    }
}