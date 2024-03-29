using System.Collections;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerState : NetworkBehaviour
{
    [Header("SO获取")]
    public PlayerData_SO playerData;
    public DevilData_SO devilData;

    [SerializeField]
    private int bulletDmg = 10;
    private int enemyDmg = 5;

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
    [SyncVar(hook = nameof(OnChangeHpUI))]
    public int currentHp;
    [ReadOnly]
    public int currentReplyVolume;
    [ReadOnly]
    public float currentReplyTime;
    float replytime;
    #endregion
    #region 魔王变量
    //体积变大
    private Vector3 initialScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 targetScale;
    public float scaleChangeSpeed = 1.0f;
    #endregion

    [SyncVar]
    public int playerId;//玩家id
    [SyncVar(hook = nameof(AddExpToRank))]
    public int currentFraction;//玩家当前分数

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
    public PlayerHpBarUI hpBarUI;
    public TMP_Text pNmaeUI;

    [Header("子物体获取")]
    public GameObject hpBarObj;
    public GameObject aliveCheckObj;
    public GameObject shadowObj;
    public GameObject gunObj;
    public GameObject expBarObj;
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
        //血量重置
        currentHp = playerData.playerHP;

        currentReplyVolume = playerData.playerReplyVolume;
        currentReplyTime = playerData.playerReplyTime;
        isDead = playerData.isDead;

        //组件获取
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GetComponent<Transform>();
        devilController = GetComponent<DevilController>();

        targetScale = initialScale * 2.0f;

        pNmaeUI.text = "P" + (1 + playerId).ToString();

        if (isServer)
        {
            Reply();
            onChangeHpUICRPC();
        }
    }

    void Update()
    {
       
    }

    /// <summary>
    /// 恢复血量（呼吸回血）
    /// TODO:呼吸回血应该也是一个网络方法
    /// </summary>
    void Reply()
    {
        //if (currentHp < currentMaxHp)
        //{
        //    replytime += Time.deltaTime;
        //    if (replytime >= currentReplyTime)
        //    {

        //    }
        //}
        //else
        //{
        //    replytime = 0;
        //}
        StartCoroutine(IEReply());
    }

    IEnumerator IEReply()
    {
        //写一个无限循环，放在start里执行
        while(true)
        {
            //如果检测血量不是满的
            if(currentHp < currentMaxHp)
            {
                //恢复指定速度的血量
                currentHp += currentReplyVolume;
                //回血间隔，和检测间隔凑一起刚好0.5f
                yield return new WaitForSeconds(0.4f);
            }
            //每隔0.1s检测一次当前血量情况
            yield return new WaitForSeconds(0.1f);
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
            StartCoroutine(ScaleOverTime(targetScale));
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
            StartCoroutine(ScaleOverTime(initialScale));
            currentMaxHp = svaeMaxHP;//还原血量
        }
    }

    void DecreaseHealth()
    {
        // 逐渐减少最大血量
        currentMaxHp -= devilData.maxHPlossCount;
    }

    private IEnumerator ScaleOverTime(Vector3 target)
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
    private void OnTriggerEnter2D(Collider2D other) //只在服务器上执行玩家收到伤害相关
    {
        if (other.CompareTag("Bullet"))   //玩家和子弹碰撞受到伤害
        {
            Debug.Log("玩家确实读到了子弹的碰撞信息。");
            //令所有客户端的该角色同步受到伤害
            if (currentHp > 0)
            {
                currentHp -= bulletDmg;
            }
            else
            {
                isDead = true;
                StartCoroutine(IEPlayerDied(0f, 0.05f));
            }
        }

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("玩家确实受到了敌人的碰撞信息。");
            // 检查玩家是否已经死亡
            if (currentHp > 0)
            {
                currentHp -= enemyDmg;
            }
            else
            {
                isDead = true;
                StartCoroutine(IEPlayerDied(0f, 0.05f));
            }
        }

        if (other.CompareTag("HpRecovery"))
        {
            Debug.Log("玩家吃到了医疗箱。");
            if(!isDead)
            {
                currentHp = currentMaxHp;
                Destroy(other.gameObject);
                NetworkServer.Destroy(other.gameObject);
            }
        }
    }

    void OnChangeHpUI(int oldValue, int newValue)
    {
        onChangeHpUICRPC();
    }

    [ClientRpc]
    void onChangeHpUICRPC()
    {
        if (currentHp >= 0)
        {
            hpBarUI.UpdateHpBar(currentHp, currentMaxHp);
        }
    }

    IEnumerator IEPlayerDied(float respawnDelay, float delay)
    {
        yield return new WaitForSeconds(respawnDelay);
        
        //关掉所有的碰撞体
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<CircleCollider2D>().enabled = false;
        GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        //关掉开枪组件
        gunObj.SetActive(false);
        //删除玩家死亡识别用组件
        Destroy(aliveCheckObj);
        PlayerDiedCRPC();
        yield return new WaitForSeconds(delay);
    }

    [ClientRpc]
    void PlayerDiedCRPC()
    {
        //关掉所有的碰撞体
        GetComponent<BoxCollider2D>().enabled = false;
        //关掉各个子物体显示
        gunObj.SetActive(false);
        hpBarObj.SetActive(false);
        shadowObj.SetActive(false);
        expBarObj.SetActive(false);

        if (isLocalPlayer)
        {   
            //首先让玩家的透明度降低
            spriteRenderer.color = new Color(1, 1, 1, 0.6f);
        }
        else
        {
            //首先让玩家的透明度降低
            spriteRenderer.color = new Color(1, 1, 1, 0f);
            //在其他人视角里关掉名称显示
            pNmaeUI.gameObject.SetActive(false);
        }
    }
    [TargetRpc]
    void RespawnCheckOn(NetworkConnection connection)
    {
        GameManager.Instance.isPlayerRespawn = true;
    }

    //void OnGUI()
    //{
    //    GUI.Box(new Rect(40f, 50f + (playerId * 50f), 110f, 25f), $"P{playerId}: {currentFraction:0000000}");
    //}

    void AddExpToRank(int oldValue, int newValue)
    {
        string playerName = "P" + (1 + playerId).ToString();
        RankManager.Instance.AddScore(playerName, currentFraction);
    }
}


