using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyData_SO enemyData;

    public float enmeySpeed//移动速度
    {
        get { if (enemyData != null) return enemyData.enemySpeed; else return 0; }
    }
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;//获取玩家位置
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();
    }

    /// <summary>
    /// 敌人移动
    /// 可能会使用A算来调整
    /// </summary>
    void EnemyMove()
    {
        Vector2 direction = player.position - transform.position;
        direction.Normalize();//斜角标准化
        transform.Translate(direction * enmeySpeed * Time.deltaTime);

    }
}
