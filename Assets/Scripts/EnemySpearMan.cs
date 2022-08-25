using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpearMan : MonoBehaviour, IEnemy
{
    public int maxHealth = 100;

    [SerializeField] private int currentHealth;

    public Animator animator;

    public Transform player;

    public Transform enemyPosition;

    public float attackRange;

    float enemyNextAttackTime = 0f;

    float enemyAttackRate = 1f;

    public float hitBoxRange;

    public Transform hitBoxPoint;

    public LayerMask playerLayer;

    public int damageDealt = 40;

    public bool isFlipped = false;

    public float moveSpeed;

    private Transform target;

    public float distanceValue;

    public int projectileDamage;

    Rigidbody2D rb;


    public GameObject playerObject;
    public GameObject enemyObject;
    public float distance;

    public PlayerMovement playerMovement;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    private void Update()
    {
        if (playerMovement.Health <= 0)
        {
            this.enabled = false;
        }
        else
        {
            this.enabled = true;
        }
        Attack();
        LookToPlayer();

        distance = Vector3.Distance(playerObject.transform.position, enemyObject.transform.position);
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
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

    void Die()
    {
        animator.SetBool("IsDead", true);
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        this.rb.isKinematic = true;
        this.rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void Attack()
    {
        if (Time.time >= enemyNextAttackTime)
        {
            if (Vector2.Distance(player.position, enemyPosition.position) <= attackRange)
            {
                animator.SetTrigger("Attack");
                enemyNextAttackTime = Time.time + 2f / enemyAttackRate;

                Collider2D hitPlayer = Physics2D.OverlapCircle(hitBoxPoint.position, hitBoxRange, playerLayer);

                if (hitPlayer != null)
                {
                    hitPlayer.GetComponent<PlayerMovement>().TakeDamage(damageDealt);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (hitBoxPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(hitBoxPoint.position, hitBoxRange);

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

    public void MoveToPlayer()
    {
        if (distance <= distanceValue && distance >= 2.2)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
            animator.SetTrigger("Walking");
        }
    }
}
