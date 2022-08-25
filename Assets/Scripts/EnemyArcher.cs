using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour, IEnemy
{
    public GameObject projectile;

    private Transform player;

    private float timeBtwShots;

    public float startTimeBtwShots;

    private Vector2 target;

    public int damage = 50;

    public bool isFlipped = false;

    public float distance;

    public float distanceValue = 20;

    public GameObject playerObject;

    public GameObject enemyObject;

    public Transform arrowPoint;

    public Animator animator;

    public int maxHealth = 100;

    [SerializeField] private int currentHealth;

    Rigidbody2D rb;

    public PlayerMovement playerMovement;

    public int projectileDamage = 50;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Attack();
        LookToPlayer();
        if (playerMovement.Health <= 0)
        {
            this.enabled = false;
        }
        else
        {
            this.enabled = true;
        }
        distance = Vector3.Distance(playerObject.transform.position, enemyObject.transform.position);

    }

    void Attack()
    {
        if (timeBtwShots < 0 && distance < distanceValue)
        {
            Vector2 direction = player.transform.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            arrowPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Instantiate(projectile, arrowPoint.position, arrowPoint.rotation);

            animator.SetTrigger("Attack");

            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    public void LookToPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        this.rb.isKinematic = true;

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamageFromProjectile()
    {
        currentHealth -= projectileDamage;

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Saw"))
        {
            TakeDamageFromProjectile();
        }
    }

}
