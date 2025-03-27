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
            // Chỉ lấy tọa độ X của người chơi, giữ nguyên tọa độ Y của kẻ thù
            Vector2 targetPosition = new Vector2(player.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }

}