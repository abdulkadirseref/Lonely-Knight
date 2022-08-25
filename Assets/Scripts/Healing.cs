using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public HealthBar healthBar;
    public float speed;

    private void Update()
    {
        transform.Rotate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UpgradeHealth();
        }
    }

    public void UpgradeHealth()
    {
        playerMovement.Health += 25;
        healthBar.SetHealth(playerMovement.Health);
        Destroy(gameObject);
    }
}
