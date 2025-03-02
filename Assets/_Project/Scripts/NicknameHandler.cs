using UnityEngine;
using Photon.Pun;
using TMPro;
using Unity.Cinemachine;

public class NicknameHandler : MonoBehaviourPun
{
    public TMP_Text nicknameText;
    public Transform nicknameTagHolder;

    void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void SetNickname(string nickname)
    {
        nicknameText.text = nickname;
    }

    void Update()
    {
        Camera activeCamera = Camera.main; // Default to Unity's Main Camera

        if (activeCamera == null)
        {
            CinemachineBrain brain = FindFirstObjectByType<CinemachineBrain>(); // Updated method
            if (brain != null)
            {
                activeCamera = brain.OutputCamera;
            }
        }

        if (activeCamera != null)
        {
            nicknameText.transform.LookAt(activeCamera.transform);
            nicknameText.transform.Rotate(0, 180, 0);
        }
    }
}
