using Unity.Netcode;
using UnityEngine;

public class NetworkButtons : MonoBehaviour
{
    private void OnGUI()
    {
        // Check if NetworkManager exists
        if (NetworkManager.Singleton == null)
        {
            GUILayout.Label("NetworkManager not found!");
            return;
        }

        GUILayout.BeginArea(new Rect(10, 10, 300, 300));

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }

        GUILayout.EndArea();
    }
}
