using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomLobby : MonoBehaviourPunCallbacks
{
    public TMP_Text RoomNameText;
    public Transform PlayerListContent;
    public GameObject PlayerListItemPrefab;

    void Start()
    {
        RoomNameText.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        foreach (Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerItem = Instantiate(PlayerListItemPrefab, PlayerListContent);
            playerItem.SetActive(true);
            playerItem.transform.GetChild(0).GetComponent<TMP_Text>().text = player.NickName;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
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
            PhotonNetwork.LoadLevel("SampleScene");
        }
    }
}
