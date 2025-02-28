using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;

public class RoomList : MonoBehaviourPunCallbacks
{
    public GameObject RoomPrefab;
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
{
    foreach (Transform child in GameObject.Find("Content").transform)
    {
        Destroy(child.gameObject); // Clear existing rooms
    }

    foreach (var roomInfo in roomList)
    {
        GameObject roomObject = Instantiate(RoomPrefab, GameObject.Find("Content").transform);
        roomObject.GetComponent<Room>().Name.text = roomInfo.Name;
        Debug.Log("Room Added: " + roomInfo.Name);
    }
}
}
