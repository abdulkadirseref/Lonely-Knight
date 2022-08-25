using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRB;
    public float moveSpeed;
    public float jumpForce;
    private float moveInput;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    private bool facingRight = false;
    public LayerMask enemyLayer;
    public int damageDealt = 50;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    Animator animator;


    public bool isGrounded = false;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundCheckLayer;

    public HealthBar healthBar;

    public GameObject projectile;

    private float timeBtwShots;

    public float startTimeBtwShots;

    public Transform projectilePoint;

    public int projectileAmount;

    public Text projectileText;

    public int Health
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        healthBar.SetMaxHealth(maxHealth);

        projectileAmount = 3;
    }

    private void Update()
    {
        Attack();
        ThrowProjectile();
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }
        projectileText.text = projectileAmount.ToString();
    }

    private void FixedUpdate()
    {
        OnGroundCheck();

        Jump();

        moveInput = Input.GetAxisRaw("Horizontal");
        playerRB.velocity = new Vector2(moveInput * moveSpeed, playerRB.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (facingRight == false && moveInput < 0)
        {
            FlipFace();
        }
        else if (facingRight == true && moveInput > 0)
        {
            FlipFace();
        }
    }

    void FlipFace()
    {
        facingRight = !facingRight;
        Vector3 tempLocalScale = transform.localScale;
        tempLocalScale.x *= -1;
        transform.localScale = tempLocalScale;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded == true)
        {
            playerRB.velocity = Vector2.up * jumpForce;
        }
        if (isGrounded == false)
        {
            animator.SetBool("IsJumping", true);
        }
        if (isGrounded == true)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    void OnGroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayer);
    }

    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<IEnemy>().TakeDamage(damageDealt);
                }
            }
        }
    }

    void ThrowProjectile()
    {

        if (timeBtwShots < 0 && Input.GetKeyDown(KeyCode.Space) && projectileAmount > 0)
        {
            GameObject playerProjectile = Instantiate(projectile, projectilePoint);
            playerProjectile.transform.parent = null;
            timeBtwShots = startTimeBtwShots;
            projectileAmount--;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamageFromArrow()
    {
        currentHealth -= 50;
        animator.SetTrigger("Hurt");

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Arrow")
        {
            TakeDamageFromArrow();
        }
    }



    public void Die()
    {
        animator.SetBool("IsDead", true);
        this.enabled = false;
        playerRB.constraints = RigidbodyConstraints2D.FreezePositionX;
        playerRB.constraints = RigidbodyConstraints2D.FreezePositionY;
    }



    public int collectedStars;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Star")
        {
            collectedStars += 1;
        }
    }

    public string original;
    public void SetStar(int levelID)
    {
        original = PlayerPrefs.GetString("stars");

        if (collectedStars > int.Parse(original.Substring((levelID - 1) * 2, 1)))
        {
            original = original.Remove((levelID - 1) * 2, 1);
            original = original.Insert((levelID - 1) * 2, collectedStars.ToString());
        }
        PlayerPrefs.SetString("stars", original);
    }
}
