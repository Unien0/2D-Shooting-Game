using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class EnemySpawner : NetworkBehaviour
{
    [System.Serializable]
    public class Wave
    {
        [Header("波次名称，可变")]
        public string waveName;
        [Header("敌人列表")]
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        [Header("时间间隔，可变")]
        public float spawnInterval;
        [Header("时间间隔差值浮动时间，敌人会有+-x的差值生成时间")]
        [Range(0, 2f)]
        public float differenceTimeInterval;
        [Header("当前数")]
        public int spawnCount;

    }

    [System.Serializable]
    public class EnemyGroup
    {
        [Header("敌人名称，可变")]
        public string enemyName;
        [Header("具体敌人生成数量，可变")]
        public int enemyCount;
        [Header("当前生成数")]
        public int spawnCount;
        [Header("敌人物体，可变")]
        public GameObject enemyPrefab;

    }

    [Header("敌人波次")]
    public List<Wave> waves;

    [Header("当前波次")]
    public int currentWaveCount;
    
    [Header("基础数值")]
    [Header("波次时间间隔，可变")]
    public float waveInterval;
    [Header("场上敌人幸存数")]
    public int enemiesAlive;
    [Header("场上的敌人最大生成数，可变")]
    public int maxEnemiesAllowed;
    [Header("是否到达最大生成量")]
    public bool maxEnemiesReached = false;
    private bool isWaveActive = false;
    float spawnTimer;

    [Header("敌人生成范围")]
    [Range (0,1f)]
    public float enemyGenerationRange;

    [Header("敌人生成点")]
    private Transform thisTransform;

    void Start()
    {
        thisTransform = GetComponent<Transform>();
        CalculateWaveQuota();
    }

    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        //在生成时会有差值
        float defferentTime = Random.Range(-waves[currentWaveCount].differenceTimeInterval, waves[currentWaveCount].differenceTimeInterval);
        if (spawnTimer >= waves[currentWaveCount].spawnInterval + defferentTime)
        {
            //如果到时间了，就将时间归零，并生成
            spawnTimer = 0f;
            SpawnEnemies();
        }

        enemiesAlive = this.transform.childCount;
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true;
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveQuota].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;

    }

    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    float spawnX = Random.Range(-enemyGenerationRange, enemyGenerationRange);
                    float spawnY = Random.Range(-enemyGenerationRange, enemyGenerationRange);
                    Vector3 spawnPosition = new Vector3(spawnX, spawnY,0);

                    EnemyInstant(enemyGroup,spawnPosition);
                    //Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10f, 10f), player.transform.position.y + Random.Range(-10f, 10f));
                    //Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
                    enemyGroup.enemyCount++;
                    waves[currentWaveCount].spawnCount++;

                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                }
            }
        }
    }

    void EnemyInstant(EnemyGroup enemyGroup,Vector3 spawnPosition)
    {
        var temp = Instantiate(enemyGroup.enemyPrefab, spawnPosition + thisTransform.position, Quaternion.identity,this.transform);
        NetworkServer.Spawn(temp);
    }
}
