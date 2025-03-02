using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPun, IPunObservable
{
    public int maxHealth = 100;
    private int currentHealth;
    private Slider uiHealthBar;  // This will be assigned dynamically

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
            else
            {
                Debug.LogError("❌ UI Health Bar not found in CanvasManager!");
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!photonView.IsMine) return;

        currentHealth -= damage;

        if (uiHealthBar != null)
        {
            uiHealthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
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
