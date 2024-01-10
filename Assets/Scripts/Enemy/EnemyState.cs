using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
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

    public bool isDead;

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
        player = FindObjectOfType<PlayerState>().transform;
        parentPool = FindObjectOfType<EnemyPool>();
    }

    void Update()
    {

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
            gameObject.layer = 9;
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
        Destroy(gameObject);
    }

}
