using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyBullet : MonoBehaviour
{
    public float testLiveTime = 3f;
    private float newTime;

    //组件获取
    private Rigidbody2D rb2D;
    private Collider2D col2d;

    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        col2d = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        Existence();
    }

    void Existence()
    {
        newTime += Time.deltaTime;
        if (newTime >= testLiveTime)
        {
            newTime = 0f;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("被打中了");

            Destroy(this.gameObject);
        }
    }
}
