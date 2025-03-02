using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Obstacle : MonoBehaviourPun, IPunObservable
{
    public int Health = 10;
    public Slider healthBarPrefab;
    private Slider healthBar;

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Create a health bar and attach it to the player
            GameObject healthBarObject = Instantiate(healthBarPrefab.gameObject, transform);
            healthBar = healthBarObject.GetComponent<Slider>();

            if (healthBar != null)
            {
                healthBar.maxValue = Health;
                healthBar.value = Health;
            }
            else
            {
                UnityEngine.Debug.LogError("HealthBar not found! Make sure it's assigned in the Inspector.");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!photonView.IsMine)
            return;

        UnityEngine.Debug.Log("Collision detected with: " + other.transform.name);

        if (other.transform.tag == "Bullet")
        {
            photonView.RPC("TakeDamage", RpcTarget.All, 2);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine)
            return;

        Health -= damage;
        UnityEngine.Debug.Log($"Health: {Health}");

        if (healthBar != null)
        {
            healthBar.value = Health;
        }

        if (Health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            Health = (int)stream.ReceiveNext();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Obstacle : MonoBehaviourPun, IPunObservable
{
    public int Health = 10;
    public Slider healthBarPrefab;
    private Slider healthBar;

    private void Start()
    {
        if (photonView.IsMine)
        {
            healthBar = Instantiate(healthBarPrefab, transform);

            if (healthBar != null)
            {
                healthBar.maxValue = Health;
                healthBar.value = Health;
            }
            else
            {
                UnityEngine.Debug.LogError("HealthBar not found! Make sure it's assigned in the Inspector.");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!photonView.IsMine)
            return;

        UnityEngine.Debug.Log("Collision detected with: " + other.transform.name);

        if (other.transform.tag == "Bullet")
        {
            photonView.RPC("TakeDamage", RpcTarget.All, 2);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine)
            return;

        Health -= damage;
        UnityEngine.Debug.Log($"Health: {Health}");

        if (healthBar != null)
        {
            healthBar.value = Health;
        }

        if (Health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else
        {
            Health = (int)stream.ReceiveNext();
        }
    }
}