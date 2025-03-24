using UnityEngine;
using System.Collections;
using TMPro;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartRespawnCountdown(int seconds, PlayerHealth player)
    {
        StartCoroutine(RespawnCoroutine(seconds, player));
    }

    private IEnumerator RespawnCoroutine(int seconds, PlayerHealth player)
    {
        if (CanvasManager.Instance != null && CanvasManager.Instance.respawnText != null)
        {
            CanvasManager.Instance.respawnText.gameObject.SetActive(true);
        }

        for (int i = seconds; i > 0; i--)
        {
            if (CanvasManager.Instance != null && CanvasManager.Instance.respawnText != null)
            {
                CanvasManager.Instance.respawnText.text = $"Respawning in {i}...";
            }
            yield return new WaitForSeconds(1f);
        }

        if (CanvasManager.Instance != null && CanvasManager.Instance.respawnText != null)
        {
            CanvasManager.Instance.respawnText.gameObject.SetActive(false);
        }

        if (PlayerSpawner.Instance != null)
        {
            PlayerSpawner.Instance.SpawnPlayer();
        }
        else
        {
            Debug.LogError("❌ PlayerSpawner Instance is NULL! Ensure it exists in the scene.");
        }
    }
}
