using UnityEngine;

public class ChaseBehavior : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void Chase()
    {
        float speed = enemyMovement.GetComponent<EnemyMovement>().speed;
        GameObject player = enemyMovement.GetPlayer();
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }
}