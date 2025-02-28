using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CamFollow : MonoBehaviour
{
    public Transform carTransform; // Assign this dynamically
    [Range(1, 10)] public float followSpeed = 2;
    [Range(1, 10)] public float lookSpeed = 5;

    private Vector3 initialCameraPosition;
    private Vector3 initialCarPosition;
    private Vector3 absoluteInitCameraPosition;

    void Start()
    {
        // Find the local player's car
        if (PhotonNetwork.InRoom)
        {
            foreach (GameObject car in GameObject.FindGameObjectsWithTag("PlayerCar"))
            {
                PhotonView pv = car.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)  // Ensure we only follow the local player
                {
                    carTransform = car.transform;
                    break;
                }
            }
        }

        if (carTransform == null)
        {
            Debug.LogError("No local player's car found for the camera to follow!");
            return;
        }

        initialCameraPosition = gameObject.transform.position;
        initialCarPosition = carTransform.position;
        absoluteInitCameraPosition = initialCameraPosition - initialCarPosition;
    }

    void FixedUpdate()
    {
        if (carTransform == null) return; // Don't do anything if no car is assigned

        // Look at car
        Vector3 _lookDirection = carTransform.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);

        // Move to car
        Vector3 _targetPos = absoluteInitCameraPosition + carTransform.transform.position;
        transform.position = Vector3.Lerp(transform.position, _targetPos, followSpeed * Time.deltaTime);
    }
}
