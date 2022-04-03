using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    int damage = 25;
    int maxHealth = 100;
    int currentHealth;

    float attackRange = 0.5f;
    float attackRate = 2f;
    float nextTimeAttack = 0f;

    public LayerMask playerLayer;
    public Transform attackPoint;
    public Animator animator;
    public Transform player;
    public bool isFlipped = false;

    #endregion

    void Start()
    {
        currentHealth = maxHealth;
    }

    #region Attack

    public void AttackToPlayer()
    {
        if (Time.time >= nextTimeAttack && !PlayerCombat.isPlayerDead)
        {
            animator.SetTrigger("Attack");

            Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

            foreach (Collider2D target in targets)
            {
                target.GetComponent<PlayerCombat>().GetDamage(damage);
            }

            nextTimeAttack = Time.time + 1f / attackRate;
        }
        else
        {
            // Idle Animation
            animator.Play("Enemy_Idle");
        }
    }

    #endregion 

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

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

    /// <summary>
    /// Player affects damage to Enemy
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Play Hurt Animation
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Play Death Animation
        animator.SetBool("IsDead", true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        // Disable the
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
