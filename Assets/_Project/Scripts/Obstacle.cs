using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    public int Health = 10;
    private Slider healthBar;

    private void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        if (healthBar != null)
        {
            healthBar.maxValue = Health;
            healthBar.value = Health;
        }
        else
        {
            Debug.LogError("HealthBar not found! Make sure it's named correctly and inside the Canvas.");
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with: " + other.transform.name);

        if (other.transform.tag == "Bullet")
        {
            Health -= 2;
            Debug.Log($"Health: {Health}");

            if (healthBar != null)
            {
                healthBar.value = Health; 
            }

            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
