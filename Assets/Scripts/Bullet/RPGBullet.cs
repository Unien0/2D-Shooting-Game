using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RPGBullet : NetworkBehaviour
{
    public PlayerBullet_SO bulletData;

    public int playerBulletDamage
    {
        get { if (bulletData != null) return bulletData.playerBulletDamage; else return 0; }
    }
    public float linerBulletDamageMultipler
    {
        get { if (bulletData != null) return bulletData.bulletDamageMultipler; else return 0; }
    }
    public int linerBulletPenetrationCount//穿透次数
    {
        get { if (bulletData != null) return bulletData.linerBulletPenetrationCount; else return 1; }
    }
    public float linerBulletExistenceTime//存在时间
    {
        get { if (bulletData != null) return bulletData.linerBulletExistenceTime; else return 3; }
    }


    //当前数据，只读
    [SerializeField]
    [ReadOnly]
    private int currentDamage = 1;
    [SerializeField]
    [ReadOnly]
    private float currentDamageMultipler = 1;
    [SerializeField]
    [ReadOnly]
    private int currentPenetrationCount = 1;
    [SerializeField]
    [ReadOnly]
    private float newTime;
    //[SerializeField][ReadOnly]
    //private bool canRelease;

    //组件获取
    private Rigidbody2D rb2D;
    private Collider2D col2d;
    //private BulletPool parentPool;
    public Collider2D explosionCol;

    private void Awake()
    {
        currentDamage = playerBulletDamage;
        currentDamageMultipler = linerBulletDamageMultipler;
        currentPenetrationCount = linerBulletPenetrationCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        col2d = gameObject.GetComponent<Collider2D>();
        //parentPool = FindObjectOfType<BulletPool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            Existence();
        }
    }

    /// <summary>
    /// 1.21孟：子弹自动消失的代码
    /// 一段时间后若是谁也没碰到就销毁
    /// </summary>
    void Existence()
    {
        newTime += Time.deltaTime;
        if (newTime >= linerBulletExistenceTime)
        {
            newTime = 0f;
            AOEExplosion();
            //parentPool.ReleaseExplosion(this);
        }
    }

    void DestoryBullet()
    {
        if (isServer)
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
    void DealDamage(GameObject enemy)
    {
        //伤害传输
        //SpaceArtPublishState spaceArtPublishState = collision.gameObject.GetComponent<SpaceArtPublishState>();
        EnemyState enemyStats = enemy.GetComponent<EnemyState>();
        TestEnemyState testEnemyState = enemy.GetComponent<TestEnemyState>();

        int damage = (int)(currentDamage * currentDamageMultipler);
        Debug.Log(damage);

        //spaceArtPublishState.TakeDamage(damage);
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(damage);
        }
        if (testEnemyState != null)
        {
            testEnemyState.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("PlayerDmg")|| other.CompareTag("Obstacle"))
        {
            AOEExplosion();
        }
    }

    /// <summary>
    /// 范围爆炸
    /// </summary>
    public void AOEExplosion()
    {
        // 检测进入爆炸范围的所有碰撞体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionCol.transform.position, explosionCol.bounds.size.x / 2f);

        foreach (Collider2D collider in colliders)
        {
            // 判断碰撞体是否为敌人
            if (collider.CompareTag("Enemy") || collider.CompareTag("PlayerDmg"))
            {
                // 调用DealDamage函数传输伤害
                DealDamage(collider.gameObject);
            }
        }
        DestoryBullet();
    }

}
