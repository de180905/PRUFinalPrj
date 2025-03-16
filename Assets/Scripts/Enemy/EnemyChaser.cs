using UnityEngine;

public class EnemyChaser : EnemyScript
{
    protected override void OnPlayerDetected()
    {      
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
