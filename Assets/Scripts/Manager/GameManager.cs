using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : Singleton<GameManager>//设置为单例
{

    [Header("玩家重生点")]
    public List<Transform> relativeSpawnPoints;

    public bool isPlayerRespawn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerRespawn)
        {
            NetworkClient.AddPlayer();
            isPlayerRespawn = false;
            FindObjectOfType<PlayerState>().isDead = false;
        }
        
    }
}
