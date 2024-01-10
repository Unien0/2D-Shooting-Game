using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
    private int playerBulletCurrentDamage = 1;
    [SerializeField]
    [ReadOnly]
    private float linerBulletCurrentDamageMultipler = 1;
    [SerializeField]
    [ReadOnly]
    private int linerBulletCurrentPenetrationCount = 1;
    [SerializeField]
    [ReadOnly]
    private float newTime;
    //[SerializeField][ReadOnly]
    //private bool canRelease;

    //组件获取
    private Rigidbody2D rb2D;
    private Collider2D col2d;
    private BulletPool parentPool;

    private void Awake()
    {
        playerBulletCurrentDamage = playerBulletDamage;
        linerBulletCurrentDamageMultipler = linerBulletDamageMultipler;
        linerBulletCurrentPenetrationCount = linerBulletPenetrationCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        col2d = gameObject.GetComponent<Collider2D>();
        parentPool = FindObjectOfType<BulletPool>();
    }

    // Update is called once per frame
    void Update()
    {
        Existence();
    }

    void Existence()
    {
        newTime += Time.deltaTime;
        if (newTime >= linerBulletExistenceTime)
        {
            newTime = 0f;
            parentPool.ReleaseExplosion(this);
        }
    }

    void DealDamage(GameObject enemy)
    {
        //伤害传输
        //SpaceArtPublishState spaceArtPublishState = collision.gameObject.GetComponent<SpaceArtPublishState>();
        EnemyState enemyStats = enemy.GetComponent<EnemyState>();
        int damage = (int)(playerBulletCurrentDamage * linerBulletCurrentDamageMultipler);
        Debug.Log(damage);
        //spaceArtPublishState.TakeDamage(damage);
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            DealDamage(other.gameObject);

            //回收
            linerBulletCurrentPenetrationCount--;
            if (linerBulletCurrentPenetrationCount <= 0)
            {
                parentPool.ReleaseExplosion(this);
            }
        }
    }

}
