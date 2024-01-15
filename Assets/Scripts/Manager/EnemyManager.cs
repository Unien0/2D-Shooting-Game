using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyManager : NetworkBehaviour
{
    //预定义敌人的预制体和生成地点
    [SerializeField] private GameObject enemySample;
    [SerializeField] private Transform spawnPos;

    public GameObject EnemySample => enemySample;
    public Transform SpawnPos => spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        if(isServer)
        {
            GameObject temp = Instantiate(EnemySample, spawnPos.position, Quaternion.identity);
            NetworkServer.Spawn(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
