using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public Button backButton; // Optional if you have a back button

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
        backButton.onClick.AddListener(BackToMainMenu);
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void BackToMainMenu()
    {
        // Load the main menu scene or disable UI elements
        Debug.Log("Returning to Main Menu");
    }
}
