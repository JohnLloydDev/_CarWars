using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class Room : MonoBehaviour
{
    public TMP_Text Name; // Change Text to TMP_Text

    public void JoinRoom()
    {
        GameObject.Find("CreateAndJoin").GetComponent<CreateAndJoin>().JoinRoomInList(Name.text);
    }
}
