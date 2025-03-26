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
    }

    public void ShootIfDetected()
    {
        if (enemyMovement == null) return;

        float distance = enemyMovement.GetDistanceToPlayer();
        if (distance <= enemyMovement.GetComponent<EnemyMovement>().detectionRange)
        {
            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                timer = 0f;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (currentBullet != null)
        {
            currentBullet.SetActive(false);
        }

        currentBullet = Instantiate(bulletPrefab, bulletPot.position, Quaternion.identity);
        currentBullet.SetActive(true);

        float direction = transform.localScale.x;
        EnemyBulletScript enemyBullet = currentBullet.GetComponent<EnemyBulletScript>();
        if (enemyBullet != null)
        {
            enemyBullet.Initialize(direction);
        }
    }
}