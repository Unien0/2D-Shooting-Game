using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyState : MonoBehaviour
{
    //敌人基本属性
    public GameObject deadEffect;   // 子弹预制体
    public float currentMoveSpeed;
    public int currentHealth = 10;
    public int currentDamage;
    public bool isDead;


    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerState>().transform;
    }

    /// <summary>
    /// 敌方个体受伤
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            //改变个体Layer至敌人尸体栏
            isDead = true;
            //anim.SetBool("Dead", true);
            Kill();
        }
    }

    /// <summary>
    /// 敌人个体死亡
    /// </summary>
    public void Kill()
    {
        Instantiate(deadEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
