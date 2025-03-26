using UnityEngine;

public class FlyingEnemyBehavior : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private ShootingBehavior shootingBehavior;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        shootingBehavior = GetComponent<ShootingBehavior>();
    }

    public void HandleFlyingEnemy()
    {
        if (enemyMovement == null) return;

        float distance = enemyMovement.GetDistanceToPlayer();
        if (distance <= enemyMovement.GetComponent<EnemyMovement>().detectionRange)
        {
            // Lật mặt về phía player
            enemyMovement.FacePlayer();

            // Bắn đạn nếu có ShootingBehavior
            if (shootingBehavior != null)
            {
                shootingBehavior.ShootIfDetected();
            }
        }
    }
}