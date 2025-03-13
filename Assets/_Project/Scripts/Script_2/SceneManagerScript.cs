using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SceneManagerScript : MonoBehaviourPunCallbacks
{
    public void LoadScene(string sceneName)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect(); // Disconnect from Photon first
            StartCoroutine(WaitForDisconnectAndLoad(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private System.Collections.IEnumerator WaitForDisconnectAndLoad(string sceneName)
    {
        while (PhotonNetwork.IsConnected) 
        {
            yield return null; // Wait until disconnected
        }
        SceneManager.LoadScene(sceneName);
    }
}
