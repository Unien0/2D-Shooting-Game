using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class PlayerState : NetworkBehaviour
{
    [Header("SO获取")]
    public PlayerData_SO playerData;
    public DevilData_SO devilData;

    [SerializeField]
    private int bulletDmg = 10;

    #region SO数据获取
    /// <summary>
    /// TODO:后期更新获取数据的方式
    /// </summary>
    //获取血量相关
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
    #endregion
    #region 当前变量
    [ReadOnly]
    public int currentMaxHp;
    [ReadOnly]
    public int currentHp;
    [ReadOnly]
    public int currentReplyVolume;
    [ReadOnly]
    public float currentReplyTime;
    float replytime;
    #endregion
    #region 魔王变量
    //体积变大
    private Vector3 initialScale;
    private Vector3 targetScale;
    public float scaleChangeSpeed = 1.0f;
    #endregion

    [ReadOnly]
    public float currentFraction;//玩家当前分数

    public bool isDead; 
    public bool highestPoint;//玩家是否是分数最高的

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
    Transform playerTransform;
    DevilController devilController;

    private void Awake()
    {
        EventCenter.AddListener<bool>(EventType.Demonization, BecomingDevil);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventType.Demonization, BecomingDevil);
    }

    void Start()
    {
        //初始化各项玩家的Parameter，从SO中读取初值
        currentMaxHp = playerData.playerMaxHP;
        currentHp = playerData.playerHP;
        currentReplyVolume = playerData.playerReplyVolume;
        currentReplyTime = playerData.playerReplyTime;
        isDead = playerData.isDead;

        //图像放大
        targetScale = initialScale * 2.5f;
        //血量重置
        currentHp = currentMaxHp;

        //组件获取
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        devilController = GetComponent<DevilController>();
    }

    void Update()
    {
        Reply();
    }

    /// <summary>
    /// 恢复血量（呼吸回血）
    /// TODO:呼吸回血应该也是一个网络方法
    /// </summary>
    void Reply()
    {
        if (currentHp < currentMaxHp)
        {
            replytime += Time.deltaTime;
            if (replytime >= currentReplyTime)
            {
                currentHp += currentReplyVolume;
            }
        }
        else
        {
            replytime = 0;
        }
    }

    /// <summary>
    /// 玩家死亡的本地做法，实现网络方法后暂时注释掉
    /// </summary>
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

    /// <summary>
    /// 获取分数（关联到根据分数来修改是否是魔王）
    /// </summary>
    /// <param name="amount"></param>
    public void GetPoint(int amount)
    {
        currentFraction += amount;
    }

    /// <summary>
    /// 玩家变成魔王
    /// </summary>
    private void BecomingDevil(bool demonization)
    {
        int svaeMaxHP = currentMaxHp;
        
        if (demonization)
        {
            StartCoroutine(ScaleOverTime(initialScale));
            currentMaxHp += devilData.devilMaxHp;//血量加成
            currentHp = currentMaxHp;//血量回复到最大

            if (currentMaxHp >= svaeMaxHP)
            {
                //在devilData.maxHPLossFrequency时间后调用减少最大血量的方法，每隔一段时间自动触发
                InvokeRepeating("DecreaseHealth", 0f, devilData.maxHPLossFrequency);
            }
            else
            {
                devilController.demonization = false;//如果最高血量小于原本的一定值时，退出魔王状态
                EventCenter.Broadcast<bool>(EventType.Demonization, devilController.demonization);
            }
        }
        else
        {
            StartCoroutine(ScaleOverTime(targetScale));
            currentMaxHp = svaeMaxHP;//还原血量
        }
    }

    void DecreaseHealth()
    {
        // 逐渐减少最大血量
        currentMaxHp -= devilData.maxHPlossCount;
    }

    private System.Collections.IEnumerator ScaleOverTime(Vector3 target)
    {
        float startTime = Time.time;

        while (Time.time - startTime < 1.0f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, (Time.time - startTime) * scaleChangeSpeed);
            yield return null;
        }

        // 确保最终缩放值准确
        transform.localScale = target;
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Bullet"))
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
        currentHp -= damage;
        Debug.Log("玩家当前生命值：" + currentHp);
        // 处理玩家死亡或其他逻辑
        if (currentHp <= 0)
        {
            isDead = true;
            //延迟0.1秒后发送销毁玩家预制体的指令给服务器
            Invoke("CmdPlayerDied", 0.1f);
            //延迟0.5秒后发送再次生成玩家预制体的指令，在各个客户端上
            GameManager.Instance.isPlayerRespawn = true;
        }
    }

    [Command]
    void CmdPlayerDied()
    {
        NetworkServer.Destroy(gameObject);
    }

    void RespawnCheckOn()
    {
        GameManager.Instance.isPlayerRespawn = true;
    }
}
