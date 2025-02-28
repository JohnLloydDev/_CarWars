using Photon.Pun;
using UnityEngine;

public class CarShooting : MonoBehaviourPun
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 50f;
    
    private new PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine) return; // Only the local player can shoot

        if (Input.GetButtonDown("Fire1")) // Left mouse click or trigger button
        {
            photonView.RPC("RPC_FireBullet", RpcTarget.All, firePoint.position, firePoint.forward);
        }
    }

    [PunRPC]
    private void RPC_FireBullet(Vector3 position, Vector3 direction)
    {
        Debug.Log("Bullet Fired at Position: " + position);

        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().linearVelocity = direction * bulletSpeed;
    }
}
