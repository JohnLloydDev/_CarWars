using Photon.Pun;
using UnityEngine;

public class CarHealth : MonoBehaviourPun
{
    public int maxHealth = 100;
    private int currentHealth;
    private new PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        currentHealth = maxHealth;
    }

    // Take damage (Only local player calls this)
    public void TakeDamage(int damage)
    {
        
        if (!photonView.IsMine) return; // Ensure only the local player calls it

        photonView.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, damage);
    }

    [PunRPC]
    private void RPC_TakeDamage(int damage)
    {
        Debug.Log("Damage Taken! Current Health: " + currentHealth);

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyCar();
        }
    }

    private void DestroyCar()
    {
        Debug.Log("Car Destroyed!");

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
