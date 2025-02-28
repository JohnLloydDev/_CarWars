using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(input_Create.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(input_Join.text);
    }

    public void JoinRoomInList(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }

    internal void JoinRoomInList(object text)
    {
        throw new NotImplementedException();
    }
}
