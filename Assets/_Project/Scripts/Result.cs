using Photon.Pun;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    public TMP_Text resultText;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("GameResult", out object result))
        {
            resultText.text = result.ToString();
        }
        else
        {
            resultText.text = "No result available.";
        }
    }
}
