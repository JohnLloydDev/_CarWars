using UnityEngine;
using Unity.Cinemachine;
using Photon.Pun;

public class CarCameraSetup : MonoBehaviourPun
{
    private void Start()
    {
        // Ensure this script runs only for the local player
        if (photonView.IsMine)
        {
            CinemachineCamera cam = FindObjectOfType<CinemachineCamera>();

            if (cam != null)
            {
                cam.Follow = transform;
                cam.LookAt = transform;
                Debug.Log("Camera set to follow local player.");
            }
            else
            {
                Debug.LogError("No CinemachineCamera found in scene!");
            }
        }
    }
}
