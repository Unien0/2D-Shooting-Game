using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyState : NetworkBehaviour
{

    //敌人状态
    public EnemyData_SO enemyData;
    public int maxHealth
    {
        get { if (enemyData != null) return enemyData.enemyHp; else return 0; }
    }

    //敌人基本属性
    [HideInInspector]
    public float currentMoveSpeed;
    [SerializeField]
    [ReadOnly]
    private int currentHealth = 1;
    [HideInInspector]
    public int currentDamage;

    public bool isDead = false;

    Transform player;
    EnemyPool parentPool;
    Animator anim;

    void Awake()
    {
        //Assign the vaiables
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
    }

    void Start()
    {
        //player = FindObjectOfType<PlayerState>().transform;
        //parentPool = FindObjectOfType<EnemyPool>();
    }

    void Update()
    {
        if(isServer)
        {
            if(isDead)
            {
                Kill();
            }
        }
        Debug.Log("生命值是："+ currentHealth);
    }

    /// <summary>
    /// 敌方个体受伤
    /// </summary>
    /// <param name="dmg"></param>
   /* public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            //改变个体Layer至敌人尸体栏
            gameObject.layer = 9;
            isDead = true;
            //anim.SetBool("Dead", true);
            Kill();
        }
    }*/

    /// <summary>
    /// 敌人个体死亡
    /// 孟1.20：更新为网络方法
    /// </summary>
     void Kill()
    {
        if (isServer)
        {
            NetworkServer.Destroy(gameObject);
        }
     }

    /// <summary>
    /// 孟1.19：敌人受伤和死亡的网络同步代码
    /// 在原有代码基础上，做出修改，可能会损失一些功能，需要再检查点和杨对接
    /// </summary>
    /// <param name="dmg"></param> 
    [Server]
    public void TakeDamage(int dmg)
    {
        Debug.Log(111);
        if (!isDead)
        {
            currentHealth -= dmg;
            Debug.Log(currentHealth);
            if (currentHealth <= 0)
            {
                //改变个体Layer至敌人尸体栏
                gameObject.layer = 9;
                isDead = true;
                //anim.SetBool("Dead", true);

            }
        }
    }
}
