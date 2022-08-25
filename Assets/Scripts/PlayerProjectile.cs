using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed;

    public int damage = 100;

    public float lifeTime;


    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        ShotProjectile();
    }

    void ShotProjectile()
    {
        transform.Translate(transform.right * transform.localScale.x * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemySpearMan") || collision.gameObject.CompareTag("EnemyArcher"))
        {
            Destroy(gameObject);
        }
    }

}

