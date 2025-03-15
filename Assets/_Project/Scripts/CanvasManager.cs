using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }
    public Slider playerHealthBar;
    public TextMeshProUGUI respawnText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
    }

    public void StartRespawnCoroutine()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(RespawnCountdown());
        }
    }

    private IEnumerator RespawnCountdown()
    {
        if (respawnText != null)
        {
            respawnText.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                respawnText.text = $"Respawning in {i}...";
                yield return new WaitForSeconds(1f);
            }

            respawnText.gameObject.SetActive(false);
        }

        if (PlayerSpawner.Instance != null) 
        {
            PlayerSpawner.Instance.SpawnPlayer();
        }
        else
        {
            Debug.LogError("PlayerSpawner.Instance is null! Cannot respawn player.");
        }
    }
}
