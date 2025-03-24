using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using TMPro;

public class PlayerHealth : MonoBehaviourPun, IPunObservable
{
    public int maxHealth = 100;
    private int currentHealth;
    private Slider uiHealthBar;
    [SerializeField] private GameObject deathParticlesPrefab; 


    void Start()
    {
        currentHealth = maxHealth;

        if (photonView.IsMine)
        {
            uiHealthBar = CanvasManager.Instance?.playerHealthBar;

            if (uiHealthBar != null)
            {
                uiHealthBar.maxValue = maxHealth;
                uiHealthBar.value = currentHealth;
            }
        }
    }

    [PunRPC]
    public void TakeDamage(float damage, int attackerId)
    {
        if (!photonView || !photonView.IsMine) return;

        if (currentHealth <= 0) return;  

        currentHealth -= Mathf.RoundToInt(damage);

        if (uiHealthBar != null)
        {
            uiHealthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die(attackerId);
        }
    }



    private void Die(int attackerId)
    {
        if (deathParticlesPrefab != null)
        {
            GameObject deathEffect = PhotonNetwork.Instantiate(deathParticlesPrefab.name, transform.position, Quaternion.identity);
            Destroy(deathEffect, 2f);
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.photonView.RPC("AddScoreRPC", RpcTarget.All, attackerId, 1);
        }

        PlayerSpawner.Instance.StartRespawnCountdown();

        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); 
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
        }
        else
        {
            currentHealth = (int)stream.ReceiveNext();
        }
    }
}
