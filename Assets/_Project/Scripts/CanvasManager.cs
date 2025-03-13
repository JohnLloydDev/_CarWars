using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; } // Singleton with encapsulation
    public Slider playerHealthBar;
    public TextMeshProUGUI respawnText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        Instance = this;
    }

    public void StartRespawnCoroutine()
    {
        if (gameObject.activeInHierarchy) // ✅ Prevent coroutine errors if Canvas is inactive
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

        if (PlayerSpawner.Instance != null) // ✅ Prevent null reference error
        {
            PlayerSpawner.Instance.SpawnPlayer();
        }
        else
        {
            Debug.LogError("PlayerSpawner.Instance is null! Cannot respawn player.");
        }
    }
}
