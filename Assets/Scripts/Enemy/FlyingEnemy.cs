using UnityEngine;

public class FlyingEnemy : EnemyShooting
{
    protected override void OnPlayerDetected()
    {
        base.OnPlayerDetected();
        FacePlayer();
    }
    protected void FacePlayer()
    {
        if ((player.transform.position.x < transform.position.x && transform.localScale.x < 0) ||
            (player.transform.position.x > transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }
}
