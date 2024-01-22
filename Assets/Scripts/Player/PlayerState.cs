using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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

    // Start is called before the first frame update
    void Start()
    {
        playerHP = playerMaxHP;
    }

    // Update is called once per frame
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
        // 处理玩家死亡的逻辑，例如显示游戏结束画面、重新开始游戏等
        // 在这里你可以根据游戏需要添加其他逻辑
        Debug.Log("Player has died!");
        Destroy(this.gameObject);
        //isDead = true;
        // 例如，你可以在这里重新加载场景
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            NetworkServer.Destroy(gameObject);
        }
    }

}
