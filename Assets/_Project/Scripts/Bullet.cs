using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Bullet : MonoBehaviourPun
{
    public Rigidbody rb;
    public GameObject Hit;
    public GameObject Fire;

    void Start()
    {
        rb.AddForce(transform.forward * 2000);
        GameObject fireEffect = Instantiate(Fire, this.transform.position, Quaternion.identity);
        Destroy(fireEffect, 2);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                PhotonView targetPhotonView = other.gameObject.GetComponent<PhotonView>();

                if (targetPhotonView != null)
                {
                    int shooterId = photonView.Owner.ActorNumber; // Get the shooter's ActorNumber

                    targetPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered, 10, shooterId);
                }
            }
        }

        GameObject hitEffect = Instantiate(Hit, transform.position, Quaternion.identity);
        Destroy(hitEffect, 2);
        Destroy(gameObject);
    }
}