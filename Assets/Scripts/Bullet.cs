using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float maxDistance = 10f;

    Rigidbody2D myRigidbody;
    PlayerMovement player;
    float xSpeed;
    Vector2 startPosition;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        startPosition = transform.position;

        myRigidbody.freezeRotation = true;
        transform.rotation = Quaternion.identity;

        Debug.Log($"Bullet spawned at {transform.position}, Active: {gameObject.activeSelf}");
    }

    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);

        float distanceTraveled = Vector2.Distance(startPosition, transform.position);
        if (distanceTraveled >= maxDistance)
        {
            gameObject.SetActive(false);
            Debug.Log("Bullet deactivated due to max distance");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Debug.Log("Bullet hit enemy and deactivated");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
        Debug.Log("Bullet collided and deactivated");
    }
}