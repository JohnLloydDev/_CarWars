using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Bullet Hit: " + other.gameObject.name); // 🔹 Debug Message
    
    if (other.CompareTag("Car"))  // Make sure Car has the "Car" tag!
    {
        Debug.Log("Car was hit!");
        other.GetComponent<PhotonView>().RPC("RPC_TakeDamage", RpcTarget.All, 10);
        Destroy(gameObject);  // Destroy bullet on impact
    }
}

}
