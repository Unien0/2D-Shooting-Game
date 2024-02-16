using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DropRateManager : NetworkBehaviour
{
    [System.Serializable]
    public class Drop
    {
        public string name;
        public int dropObjectID;
        public GameObject itemPrefab;
        [Range(0,100)]public float dropRate;
    }
    public List<Drop> rateDrops;

    private void OnDestroy()
    {
        //如果是换scene导致的destroy，不做掉落处理
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        //rateDrops对应的是不一定掉落，但可能掉落复数个道具的处理方式
        //具体每个item存在一个掉率，调用方法时生成一个0-100之间的随机数，通过遍历得到本次可以掉落的item
        float randomNumber = Random.Range(0f, 100f);
        foreach (Drop rateDrop in rateDrops)
        {
            if (randomNumber <= rateDrop.dropRate)
            {
                DropsInstant(rateDrop);
            }
        }

        //possibleDrops对应一定会掉落，但是只掉落其中一件物品的方式
        //可以定义为公开型的成员变量，在外部赋值，也可以像现在这样做成局部变量，用相关代码为其add元素
        //通过随机数从这些item中取出一个掉落
        List<Drop> possibleDrops = new List<Drop>();
        if (possibleDrops.Count >0)
        {
            Drop possibleDrop = possibleDrops[Random.Range(0, possibleDrops.Count)];
            DropsInstant(possibleDrop);
        }
    }

    //掉落物的生成只在服务器上执行，然后广播到各客户端
    void DropsInstant(Drop drop)
    {
        if(isServer)
        {
            var temp = Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(temp);
        }
    }
}
