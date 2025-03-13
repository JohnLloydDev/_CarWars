using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance; // ✅ Singleton instance

    public GameObject[] characterPrefabs;
    public Transform[] spawnPoints;

    private static List<int> usedSpawnIndices = new List<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        if (usedSpawnIndices.Count >= spawnPoints.Length)
        {
            usedSpawnIndices.Clear();
        }

        int index;
        do
        {
            index = Random.Range(0, spawnPoints.Length);
        } while (usedSpawnIndices.Contains(index));

        usedSpawnIndices.Add(index);
        Transform spawnPoint = spawnPoints[index];

        string selectedCharacter = PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("character") ?
                                   PhotonNetwork.LocalPlayer.CustomProperties["character"].ToString() :
                                   characterPrefabs[0].name;

        GameObject selectedPrefab = System.Array.Find(characterPrefabs, prefab => prefab.name == selectedCharacter);

        if (selectedPrefab == null)
        {
            Debug.LogError($"Selected car '{selectedCharacter}' not found! Spawning default car.");
            selectedPrefab = characterPrefabs[0];
        }

        GameObject playerCar = PhotonNetwork.Instantiate(selectedPrefab.name, spawnPoint.position, spawnPoint.rotation);

        PhotonView photonView = playerCar.GetComponent<PhotonView>();
        if (photonView != null)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }

    public void StartRespawnCountdown()
    {
        StartCoroutine(RespawnCountdown());
    }

    private IEnumerator RespawnCountdown()
    {
        if (CanvasManager.Instance != null && CanvasManager.Instance.respawnText != null)
        {
            CanvasManager.Instance.respawnText.gameObject.SetActive(true);

            for (int i = 3; i > 0; i--)
            {
                CanvasManager.Instance.respawnText.text = $"Respawning in {i}...";
                yield return new WaitForSeconds(1f);
            }

            CanvasManager.Instance.respawnText.gameObject.SetActive(false);
        }

        SpawnPlayer();
    }
}
