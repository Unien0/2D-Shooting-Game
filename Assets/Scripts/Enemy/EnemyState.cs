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

    public Transform parentTransform;
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
        //时刻检查是否死亡，且只在服务器上检测
        if(isServer)
        {
            if(isDead)
            {
                Kill();
            }
        }
        //Debug.Log("生命值是："+ currentHealth);
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
        //只在服务器上销毁死去的敌人，并广播到各客户端
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
    //Server关键字是只在服务器上运行的代码，并阻止其在客户端调用
    //暂时不需要显示野怪的血量，因此无需考虑向各客户端同步野怪当前状态，只在服务器判定其死活并广播死活状态即可
    [Server]
    public void TakeDamage(int dmg)
    {
        //Debug.Log(111);
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

    /// <summary>
    /// 敌人死亡时调用，将其回收
    /// </summary>
    //private void OnDestroy()
    //{
    //    EnemySpawner es = FindObjectOfType<EnemySpawner>();
    //    es.OnEnemyKilled();
    //}
}
