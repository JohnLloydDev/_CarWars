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

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.forward * 2000);
        GameObject fireEffect = Instantiate(Fire, this.transform.position, Quaternion.identity);
        Destroy(fireEffect, 2);
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject hitEffect = Instantiate(Hit, this.transform.position, Quaternion.identity);
        Destroy(hitEffect, 2);
        Destroy(this.gameObject);
    }
}