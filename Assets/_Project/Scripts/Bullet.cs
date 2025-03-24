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

    public float damage = 10f; 
    public float range = 100f; 

    void Start()
    {
        Rigidbody carRb = transform.root.GetComponent<Rigidbody>(); 

        if (carRb != null)
        {
            rb.linearVelocity = (transform.forward * 300f) + carRb.linearVelocity;
        }
        else
        {
            rb.linearVelocity = transform.forward * 300f;
        }

        GameObject fireEffect = Instantiate(Fire, transform.position, Quaternion.identity);
        Destroy(fireEffect, 2);

        Destroy(gameObject, range / 100f);
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
                    int shooterId = photonView.Owner.ActorNumber;

                    targetPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage, shooterId);
                }
            }
        }

        GameObject hitEffect = Instantiate(Hit, transform.position, Quaternion.identity);
        Destroy(hitEffect, 2);
        Destroy(gameObject);
    }
}
