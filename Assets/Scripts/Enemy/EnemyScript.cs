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

    [SerializeField] float damage = 20f;
    public float GetDamage()
    {
        return damage;
    }

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

        if (enemyMovement != null)
        {
            enemyMovement.detectionRange = detectionRange;
        }
    }

    protected virtual void Update()
    {
        if (enemyMovement == null)
        {
            Debug.LogWarning($"EnemyMovement not found on {gameObject.name}");
            return;
        }

        float distance = enemyMovement.GetDistanceToPlayer();
        if (distance <= detectionRange)
        {
            enemyMovement.FacePlayer();
            if (flyingEnemyBehavior != null)
            {
                Debug.Log($"{gameObject.name} - Using FlyingEnemyBehavior");
                flyingEnemyBehavior.HandleFlyingEnemy();
            }
            else if (shootingBehavior != null)
            {
                Debug.Log($"{gameObject.name} - Using ShootingBehavior");
                shootingBehavior.ShootIfDetected();
            }
            else if (chaseBehavior != null)
            {
                Debug.Log($"{gameObject.name} - Using ChaseBehavior");
                enemyMovement.UpdateMovement(chaseBehavior.Chase);
            }
        }
        else if (patrolBehavior != null)
        {
            Debug.Log($"{gameObject.name} - Using PatrolBehavior");
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