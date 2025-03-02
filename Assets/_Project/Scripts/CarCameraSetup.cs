using UnityEngine;
using Unity.Cinemachine;
using Photon.Pun;

public class CarCameraSetup : MonoBehaviourPun
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            CinemachineCamera cam = Object.FindFirstObjectByType<CinemachineCamera>();

            if (cam != null)
            {
                cam.Follow = transform;
                cam.LookAt = transform;
                Debug.Log("Chase camera set up for local player.");
            }
            else
            {
                Debug.LogError("No CinemachineCamera found in the scene!");
            }
        }
    }
}
