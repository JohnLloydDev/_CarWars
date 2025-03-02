using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;  
    public Transform[] spawnPoints;  

    private static List<int> usedSpawnIndices = new List<int>();

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            SpawnPlayer();
        }
    }

    void SpawnPlayer()
    {
        if (usedSpawnIndices.Count >= spawnPoints.Length)
        {
            Debug.LogWarning("All spawn points are used, resetting...");
            usedSpawnIndices.Clear();
        }

        int index;
        do
        {
            index = Random.Range(0, spawnPoints.Length);
        } while (usedSpawnIndices.Contains(index));

        usedSpawnIndices.Add(index);

        Transform spawnPoint = spawnPoints[index];
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }
}
