using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Nickname;
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void SetNickname()
    {
        if (!string.IsNullOrEmpty(input_Nickname.text))
        {
            PhotonNetwork.NickName = input_Nickname.text;
        }
        else
        {
            PhotonNetwork.NickName = "Player" + UnityEngine.Random.Range(1000, 9999);
        }
    }

    public void CreateRoom()
    {
        SetNickname();
        PhotonNetwork.CreateRoom(input_Create.text);
    }

    public void JoinRoom()
    {
        SetNickname();
        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public void JoinRoomInList(string RoomName)
    {
        SetNickname();
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("RoomLobby");
    }
}