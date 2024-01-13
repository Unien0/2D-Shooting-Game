using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyAttack : MonoBehaviour
{
    Transform player;          // 玩家对象的Transform组件
    public GameObject bulletPrefab;   // 子弹预制体
    public Transform bulletSpawnPoint; // 子弹生成点
    public float bulletForce = 10f;    // 子弹的推力
    public float shootInterval = 0.5f;   // 射击间隔时间
    public float detectionDistance = 5f; // 检测玩家距离

    void Start()
    {
        // 获取玩家对象的Transform组件
        player = FindObjectOfType<PlayerState>()?.transform;
        // 启动协程，定期发射子弹
        StartCoroutine(ShootAtPlayer());
    }

    IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            // 计算敌人到玩家的距离
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            // 如果距离小于检测距离，发射子弹
            if (distanceToPlayer < detectionDistance)
            {
                // 创建子弹
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

                // 计算朝向玩家的方向
                Vector2 direction = (player.position - transform.position).normalized;

                // 使用AddForce方法为子弹添加推力
                bullet.GetComponent<Rigidbody2D>().AddForce(direction * bulletForce, ForceMode2D.Impulse);
            }

            // 在间隔时间后再次检测距离并发射子弹
            yield return new WaitForSeconds(shootInterval);
        }
    }
}
