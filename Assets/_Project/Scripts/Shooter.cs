using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooter : MonoBehaviourPun
{
    public GameObject Bullet;
    public GameObject FirePosition;
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    public Texture2D ammoIcon;
    public Font customFont;

    public bool autoFire = false;
    public float fireRate = 0.1f; 
    private float nextFireTime = 0f;

    void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (!photonView.IsMine || isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (autoFire && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shooting();
        }
        else if (!autoFire && Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public void Shooting()
    {
        if (isReloading)
            return;

        if (currentAmmo > 0)
        {
            PhotonNetwork.Instantiate(Bullet.name, FirePosition.transform.position, FirePosition.transform.rotation);
            currentAmmo--;
        }
    }

    void OnGUI()
    {
        if (!photonView.IsMine)
            return;

        GUIStyle style = new GUIStyle();
        style.font = customFont; 
        style.fontSize = 48; 
        style.fontStyle = FontStyle.Bold;

        float iconSize = 100;

        if (ammoIcon != null)
        {
            GUI.DrawTexture(new Rect(Screen.width - 300, Screen.height - 100, iconSize, iconSize), ammoIcon);
        }

        if (isReloading)
        {
            style.normal.textColor = Color.yellow;
            GUI.Label(new Rect(Screen.width - 300, Screen.height - 100, 200, 50), "Reloading...", style);
        }
        else
        {
            Color ammoColor = Color.Lerp(Color.red, Color.white, (float)currentAmmo / maxAmmo);
            style.normal.textColor = ammoColor;
            GUI.Label(new Rect(Screen.width - 200, Screen.height - 110, 100, 50), currentAmmo.ToString(), style);
            GUI.Label(new Rect(Screen.width - 150, Screen.height - 100, 100, 50), "/" + maxAmmo, style);
        }
    }
}