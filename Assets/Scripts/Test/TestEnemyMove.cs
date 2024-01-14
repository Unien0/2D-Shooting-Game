using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMove : MonoBehaviour
{
    Transform player;
    public float followRadius = 3f;//追逐半径
    public float escapeRadius = 2f;
    public float moveSpeed = 2f;
    public float maxSpeed = 2.5f;
    public float randomMoveDuration = 3f;
    public float stopDuration = 2f; // 时间间隔，停顿一段时间后再次判定左移还是右移
    private bool isRandomMoving = false;
    private Rigidbody2D rb;
    bool isDead;

    private void Start()
    {
        player = FindObjectOfType<PlayerState>().transform;
        rb = GetComponent<Rigidbody2D>();
        isDead = GetComponent<TestEnemyMove>().isDead;
        //环绕移动
        StartCoroutine(RandomMoveCoroutine());
    }


    void Update()
    {
        if (player != null &&!isDead)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // 如果距离大于追逐半径，向玩家施加推力
            if (distanceToPlayer > followRadius)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                Vector2 force = direction * moveSpeed;

                // 检查当前速度是否超过最大速度
                if (rb.velocity.magnitude < maxSpeed)
                {
                    // 使用推力移动
                    rb.AddForce(force);
                }
            }
            // 如果距离小于逃离半径，向相反方向施加推力
            else if (distanceToPlayer < escapeRadius)
            {
                Vector2 direction = (transform.position - player.position).normalized;
                Vector2 force = direction * moveSpeed;

                // 检查当前速度是否超过最大速度
                if (rb.velocity.magnitude < maxSpeed)
                {
                    // 使用推力移动
                    rb.AddForce(force);
                }
            }

        }
    }

    IEnumerator RandomMoveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(stopDuration);

            // 随机选择左移还是右移
            int randomDirection = Random.Range(0, 2) == 0 ? -1 : 1;
            Vector2 randomMoveDirection = new Vector2(randomDirection, 0f);

            // 缓慢持续移动
            float timer = 0f;
            while (timer < randomMoveDuration)
            {
                // 如果不在追逐状态，进行随机移动
                if (!isRandomMoving && rb.velocity.magnitude < maxSpeed)
                {
                    rb.AddForce(randomMoveDirection * moveSpeed);
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
