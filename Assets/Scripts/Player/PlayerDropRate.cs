using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropRate : MonoBehaviour
{
    //获取组件
    PlayerLevel playerLevel;
    public float positionOffsetRange = 1.0f;
    [System.Serializable]
    public class Drops
    {
        public string name;
        public int dropObjectID;
        public GameObject itemPrefab;
        [Range(0, 100)] public float dropRate;
    }
    public List<Drops> drops;

    private void Start()
    {
        playerLevel = GetComponent<PlayerLevel>();
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            return;
        }
        int levelPoint = playerLevel.experience;
        foreach (Drops rate in drops)
        {
            float randomNumber = Random.Range(0f, 100f);
            if (randomNumber <= rate.dropRate)
            {
                ExperienceGem experienceGem = rate.itemPrefab.GetComponent<ExperienceGem>();
                if (experienceGem != null)
                {
                    int experienceGranted = experienceGem.experienceGranted;
                    int dropObjectCount = levelPoint / experienceGranted;
                    for (int i = 0; i < dropObjectCount; i++)
                    {
                        //范围位置偏移量
                        Vector3 offset = new Vector3(Random.Range(-positionOffsetRange, positionOffsetRange),Random.Range(-positionOffsetRange, positionOffsetRange),0);
                        Instantiate(rate.itemPrefab, transform.position + offset, Quaternion.identity);
                    }
                }
            }
        }
        List<Drops> possibleDrops = new List<Drops>();
        foreach (Drops rate in drops)
        {
            float randomNumber = Random.Range(0f, 100f);
            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        if (possibleDrops.Count > 0)
        {
            Drops selectedDrop = possibleDrops[Random.Range(0, possibleDrops.Count)];
            Vector3 offset = new Vector3(Random.Range(-positionOffsetRange, positionOffsetRange), Random.Range(-positionOffsetRange, positionOffsetRange), 0);
            Instantiate(selectedDrop.itemPrefab, transform.position + offset, Quaternion.identity);
        }
    }
}
