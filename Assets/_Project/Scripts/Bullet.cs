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

            if (playerHealth != null && other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 10);
            }
        }

        GameObject hitEffect = Instantiate(Hit, transform.position, Quaternion.identity);
        Destroy(hitEffect, 2);
        Destroy(gameObject);
    }
}