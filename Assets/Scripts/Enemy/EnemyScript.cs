using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] float damageOnPlayerCollision = 1f;
    private HealthSystem healthSystem;
    private EnemyMovement enemyMovement;
    private PatrolBehavior patrolBehavior;
    private ChaseBehavior chaseBehavior;
    private ShootingBehavior shootingBehavior;
    private FlyingEnemyBehavior flyingEnemyBehavior;
    [SerializeField] float detectionRange = 5f;

    protected virtual void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        enemyMovement = GetComponent<EnemyMovement>();
        patrolBehavior = GetComponent<PatrolBehavior>();
        chaseBehavior = GetComponent<ChaseBehavior>();
        shootingBehavior = GetComponent<ShootingBehavior>();
        flyingEnemyBehavior = GetComponent<FlyingEnemyBehavior>();

        if (healthSystem != null)
        {
            healthSystem.OnDeath += OnDeath;
        }
    }

    protected virtual void Update()
    {
        if (enemyMovement == null) return;

        float distance = enemyMovement.GetDistanceToPlayer();
        if (distance <= detectionRange)
        {
            // Ưu tiên hành vi của FlyingEnemy nếu có
            if (flyingEnemyBehavior != null)
            {
                flyingEnemyBehavior.HandleFlyingEnemy();
            }
            // Nếu không, thực hiện hành vi bắn hoặc đuổi theo
            else if (shootingBehavior != null)
            {
                shootingBehavior.ShootIfDetected();
            }
            else if (chaseBehavior != null)
            {
                enemyMovement.UpdateMovement(chaseBehavior.Chase);
            }
        }
        else if (patrolBehavior != null)
        {
            enemyMovement.UpdateMovement(patrolBehavior.Patrol);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageOnPlayerCollision);
            }

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damageOnPlayerCollision);
            }
        }
    }

    protected virtual void OnDeath()
    {
        // Có thể thêm logic khi chết
    }
}