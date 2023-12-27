using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public PlayerData_SO playerData;

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
    /// 恢复血量（呼吸回血
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

    /// <summary>
    /// 玩家受伤脚本
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        playerHP -= damage;

        // 处理玩家死亡或其他逻辑
        if (playerHP <= 0)
        {
            Die();
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
}
