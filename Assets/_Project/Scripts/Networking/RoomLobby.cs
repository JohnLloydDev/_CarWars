using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomLobby : MonoBehaviourPunCallbacks
{
    public TMP_Text RoomNameText;
    public Transform PlayerListContent;
    public GameObject PlayerListItemPrefab;
    public GameObject startButton;

    public Transform characterSelectionPanel;  
    public GameObject characterButtonPrefab;  
    public List<GameObject> characterPrefabs; 

    private GameObject currentPreview;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.LogError("Not in a room. Returning to lobby.");
            SceneManager.LoadScene("CreateAndJoin");
            return;
        }

        if (photonView == null)
        {
            Debug.LogError("PhotonView is missing on RoomLobby! Make sure it's attached.");
            return;
        }

        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        PopulateCharacterSelection();

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdatePlayerListRPC", RpcTarget.AllBuffered);
        }

        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }


    [PunRPC]
    void UpdatePlayerListRPC()
    {
        foreach (Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerItem = Instantiate(PlayerListItemPrefab, PlayerListContent);
            playerItem.SetActive(true);
            TMP_Text playerNameText = playerItem.transform.GetChild(0).GetComponent<TMP_Text>();

            string character = player.CustomProperties.ContainsKey("character") ?
                               player.CustomProperties["character"].ToString() : "Not Selected";

            playerNameText.text = $"{player.NickName} - {character}";
        }
    }

    void PopulateCharacterSelection()
    {
        foreach (GameObject characterPrefab in characterPrefabs)
        {
            GameObject button = Instantiate(characterButtonPrefab, characterSelectionPanel);
            button.GetComponentInChildren<TMP_Text>().text = characterPrefab.name;

            Image buttonImage = button.GetComponent<Image>();
            Sprite characterSprite = characterPrefab.GetComponentInChildren<SpriteRenderer>()?.sprite;
            if (characterSprite != null) buttonImage.sprite = characterSprite;

            button.GetComponent<Button>().onClick.AddListener(() => SelectCharacter(characterPrefab.name));
        }
    }

    public void SelectCharacter(string characterName)
    {
        if (currentPreview != null)
        {
            Destroy(currentPreview);
        }

        GameObject characterPrefab = characterPrefabs.Find(c => c.name == characterName);
        if (characterPrefab != null)
        {
            currentPreview = Instantiate(characterPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            Transform modelTransform = currentPreview.transform.GetChild(0); 

            Rigidbody rb = modelTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            currentPreview.transform.position = new Vector3(3, 1.5f, 0);
            modelTransform.localScale = new Vector3(1f, 1f, 1f); 

            modelTransform.rotation = Quaternion.Euler(0, 170, 0);
        }

        Hashtable playerProperties = new Hashtable { { "character", characterName } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        photonView.RPC("UpdatePlayerListRPC", RpcTarget.AllBuffered);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("UpdatePlayerListRPC", RpcTarget.AllBuffered);
        CheckMasterClient();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("UpdatePlayerListRPC", RpcTarget.AllBuffered);
        CheckMasterClient();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("CreateAndJoin");
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Neon_Map_11");
        }
    }

    void CheckMasterClient()
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
}
