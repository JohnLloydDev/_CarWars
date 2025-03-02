using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Obstacle : MonoBehaviourPun, IPunObservable
{
    public int Health = 10;
    public GameObject healthBarPrefab;  

    private Slider healthBar;
    private Transform healthBarsContainer;

    private void Start()
    {
        if (photonView.IsMine)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null)
            {
                Debug.LogError("❌ No Canvas found! Make sure there is a UI Canvas in the scene.");
                return;
            }

            healthBarsContainer = canvas.transform.Find("HealthBars");
            if (healthBarsContainer == null)
            {
                Debug.LogError("❌ No 'HealthBars' container found! Make sure to create it inside Canvas.");
                return;
            }

            GameObject healthBarObject = Instantiate(healthBarPrefab, healthBarsContainer);
            healthBar = healthBarObject.GetComponent<Slider>();

            if (healthBar != null)
            {
                healthBar.maxValue = Health;
                healthBar.value = Health;
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine && healthBar != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f);
            healthBar.transform.position = screenPos;
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
