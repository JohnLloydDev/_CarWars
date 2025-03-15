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
        if (!photonView.IsMine) return;

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
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.photonView.RPC("AddScoreRPC", RpcTarget.All, attackerId, 1);
        }

        PlayerSpawner.Instance.StartRespawnCountdown();
        PhotonNetwork.Destroy(gameObject);
    }

    // ✅ Fix: Properly place the OnPhotonSerializeView method inside the class
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
