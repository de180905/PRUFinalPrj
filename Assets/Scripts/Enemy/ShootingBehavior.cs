using UnityEngine;

public class ShootingBehavior : MonoBehaviour
{
    [SerializeField] Transform bulletPot;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float shootInterval = 2f;
    private float timer;
    private EnemyMovement enemyMovement;
    private GameObject currentBullet;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        timer = 0f;

        if (bulletPot == null)
        {
            Debug.LogError($"Bullet Pot is not assigned in ShootingBehavior on {gameObject.name}!");
        }
        if (bulletPrefab == null)
        {
            Debug.LogError($"Bullet Prefab is not assigned in ShootingBehavior on {gameObject.name}!");
        }
    }

    public void ShootIfDetected()
    {
        if (enemyMovement == null)
        {
            Debug.LogWarning($"EnemyMovement not found on {gameObject.name}");
            return;
        }

        if (enemyMovement.CanSeePlayer())
        {
            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                timer = 0f;
                Shoot();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    void Shoot()
    {
        if (currentBullet != null)
        {
            currentBullet.SetActive(false);
        }

        if (bulletPot != null && bulletPrefab != null)
        {
            currentBullet = Instantiate(bulletPrefab, bulletPot.position, Quaternion.identity);
            currentBullet.SetActive(true);

            GameObject player = enemyMovement.GetPlayer();
            if (player != null)
            {
                // Tính vector hướng từ enemy đến player
                Vector2 directionToPlayer = (player.transform.position - bulletPot.position).normalized;
                EnemyBulletScript enemyBullet = currentBullet.GetComponent<EnemyBulletScript>();
                if (enemyBullet != null)
                {
                    enemyBullet.Initialize(directionToPlayer); // Truyền Vector2 thay vì float
                }
                else
                {
                    Debug.LogError("EnemyBulletScript not found on instantiated bullet!");
                }
            }
            else
            {
                Debug.LogWarning($"Player not found when shooting from {gameObject.name}");
            }
        }
    }
}