using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class PlayerState : NetworkBehaviour
{
    public PlayerData_SO playerData;

    [SerializeField]
    private int bulletDmg = 10;

    #region SO数据获取
    //获取血量相关
    public int playerMaxHP//最高血量
    {
        get { if (playerData != null) return playerData.playerMaxHP; else return 0; }
    }
    public int playerHP//当前血量
    {
        get { if (playerData != null) return playerData.playerHP; else return 0; }
        set { playerData.playerHP = value; }
    }
    public int playerReplyVolume//回复量
    {
        get { if (playerData != null) return playerData.playerReplyVolume; else return 0; }
    }
    public float playerReplyTime//回复启动所需时间
    {
        get { if (playerData != null) return playerData.playerReplyTime; else return 0; }
    }
    //是否死亡
    public bool isDead
    {
        get { if (playerData != null) return playerData.isDead; else return true; }
        set { playerData.isDead = value; }
    }
    #endregion

    float replytime;

    [Header("I-Frames")]
    public float invincibilityDuration;//无敌时间
    float invincibilityTimer;//无敌倒计时
    bool isInvincible;//是否无敌

    [Header("死亡处理相关")]
    public float rebirthTime;//重生时间（可变动
    float resurrectionTimer;//复活计时器
    public TMP_Text rebirthTimeDisplay;//重生时间显示

    [Header("组件获取")]
    SpriteRenderer spriteRenderer;

    void Start()
    {
        //血量重置
        playerHP = playerMaxHP;

        //组件获取
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        Reply();
        Die();
    }

    /// <summary>
    /// 恢复血量（呼吸回血）
    /// TODO:呼吸回血应该也是一个网络方法
    /// </summary>
    void Reply()
    {
        if (playerHP<playerMaxHP)
        {
            replytime += Time.deltaTime;
            if (replytime >= playerReplyTime)
            {
                playerHP += playerReplyVolume;
            }
        }
        else if (playerHP == playerMaxHP)
        {
            replytime = 0;
        }
    }

    void Die()
    {
        if (isDead)
        {
            //玩家透明化，并且将其无法移动
            spriteRenderer.color = new Color(1, 1, 1, 0);
            //进行转移与经验值清空处理
            //跳转至：PlayerMovement、PlayerLevel
            EventCenter.Broadcast(EventType.isDead);
            resurrectionTimer += Time.deltaTime;
            if (resurrectionTimer >= rebirthTime)
            {
                //复原
                spriteRenderer.color = new Color(1, 1, 1, 1);
                //重生后处理
                isDead = false;
                resurrectionTimer = 0f;
            }
        }
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Bullet"))
        {
            Debug.Log("玩家确实读到了子弹的碰撞信息。");
            //令所有客户端的该角色同步受到伤害
            TakeDamage(bulletDmg);
        }
    }
    
    /// <summary>
    /// 玩家受伤脚本
    /// 孟1.22：修改该方法为局域网通信方法
    /// </summary>
    /// <param name="damage"></param>
    [ClientRpc]
    public void TakeDamage(int damage)
    {
        Debug.Log("确实进入了受伤的代码");
        playerHP -= damage;
        Debug.Log("玩家当前生命值：" + playerHP);
        // 处理玩家死亡或其他逻辑
        if (playerHP <= 0)
        {
            isDead = true;
            //NetworkServer.Destroy(gameObject);
        }
    }

}
