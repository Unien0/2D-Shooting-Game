using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    CircleCollider2D playerCollector;
    public float pullSpeed;

    private void Start()
    {
        playerCollector = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        //设置收取范围，之后有必要可以进行调整
        playerCollector.radius =1.5f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out Icollectible collectible))
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            rb.AddForce(forceDirection * pullSpeed);

            collectible.Collect();
        }
    }
}
