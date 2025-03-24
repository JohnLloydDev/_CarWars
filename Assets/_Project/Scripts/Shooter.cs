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
    public Texture2D crosshairTexture; 
    public Font customFont;

    public bool autoFire = false;
    public float fireRate = 0.1f;
    private float nextFireTime = 0f;

    public AudioClip fireSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }

        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        if (reloadSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

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
            currentAmmo--;

            Camera cam = Camera.main;
            if (cam == null) return;

            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2 - 50, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point; 
            }
            else
            {
                targetPoint = ray.GetPoint(100f); 
            }

            Vector3 shootDirection = (targetPoint - FirePosition.transform.position).normalized;

            GameObject bulletInstance = PhotonNetwork.Instantiate(Bullet.name, FirePosition.transform.position, Quaternion.LookRotation(shootDirection));
        }

        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound);
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

        if (crosshairTexture != null)
        {
            float crosshairSize = 40; 
            float x = (Screen.width - crosshairSize) / 2;
            float y = (Screen.height - crosshairSize) / 2 + 50;
            GUI.DrawTexture(new Rect(x, y, crosshairSize, crosshairSize), crosshairTexture);
        }
    }
}
