using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Animator animator;
    Rigidbody2D rb;
    Enemy enemy;

    public int damage = 20;
    public int currentHealth;
    public int maxHealth = 100;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    public float nextTimeAttack = 0f;
    public static bool isPlayerDead;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public HealthBar healthBar;

    #endregion

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        isPlayerDead = false;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    void Update()
    {
        if (Time.time >= nextTimeAttack)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextTimeAttack = Time.time + 1f / attackRate;
            }
        }
    }

    /// <summary>
    /// Player attack to Enemy
    /// </summary>
    void Attack()
    {
        // Play attack animation
        animator.SetTrigger("Attack");

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage Enemies
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    /// <summary>
    /// Player gets damage from Enemy
    /// </summary>
    /// <param name="damage"></param>
    public void GetDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Player Die
    /// Die animation and disable collider
    /// </summary>
    void Die()
    {
        //die animation
        animator.SetBool("IsDead", true);
        rb.bodyType = RigidbodyType2D.Kinematic;

        //disable the player
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        isPlayerDead = true;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
