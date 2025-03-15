using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

public class Sword : MonoBehaviourPun
{
    public float damageAmount = 20f; 
    public float hitCooldown = 1.0f; 

    private HashSet<int> recentlyHitPlayers = new HashSet<int>();

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return; 

        PlayerHealth enemyHealth = other.GetComponentInParent<PlayerHealth>();

        if (enemyHealth != null && other.gameObject != transform.root.gameObject)
        {
            int attackerId = PhotonNetwork.LocalPlayer.ActorNumber;
            int enemyId = enemyHealth.photonView.Owner.ActorNumber; 
            if (recentlyHitPlayers.Contains(enemyId)) return;

            Debug.Log($"Sword hit: {other.gameObject.name}, Attacker ID: {attackerId}");

            if (enemyHealth.photonView != null)
            {
                enemyHealth.photonView.RPC("TakeDamage", RpcTarget.All, damageAmount, attackerId);
                Debug.Log($"Sent TakeDamage RPC to {enemyHealth.gameObject.name} for {damageAmount} damage.");

                recentlyHitPlayers.Add(enemyId);
                StartCoroutine(RemoveFromCooldown(enemyId, hitCooldown));
            }
            else
            {
                Debug.LogError($"No PhotonView found on {enemyHealth.gameObject.name}, cannot send TakeDamage RPC!");
            }
        }
    }

    private IEnumerator RemoveFromCooldown(int enemyId, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        recentlyHitPlayers.Remove(enemyId);
    }
}
