using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : Singleton<GameManager>//设置为单例
{

    [Header("玩家重生点")]
    public List<Transform> relativeSpawnPoints;

    public bool isPlayerRespawn = false;

    //游戏时间进程
    [ReadOnly]
    public float gameTime;
    //什么时候出现魔王
    [ReadOnly]
    public float isDevilTime;
    //在魔王出现期间内开启
    [ReadOnly]
    public bool devilOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && isPlayerRespawn)
        {
            NetworkClient.AddPlayer();
            isPlayerRespawn = false;
            FindObjectOfType<PlayerState>().isDead = false;
        }

        gameTime += Time.deltaTime;
        DevilTime();
    }

    void DevilTime()
    {
        if (gameTime >= isDevilTime)
        {
            devilOpen = true;
        }
    }
}
