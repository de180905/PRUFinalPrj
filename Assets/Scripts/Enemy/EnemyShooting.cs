using UnityEngine;

public class EnemyShooting : EnemyScript
{
    public Transform bulletPot;
    public GameObject bullet;
    public float shootInterval = 2;
    private float timer;
    
    protected override void OnPlayerDetected()
    {
        timer += Time.deltaTime;
        if (timer > shootInterval)
        {
            timer = 0;
            Shooting();
        }
    }
    void Shooting()
    {
        Instantiate(bullet, bulletPot.position, Quaternion.identity);
    }
}
