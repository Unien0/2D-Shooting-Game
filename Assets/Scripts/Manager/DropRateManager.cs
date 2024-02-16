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
        //����ǻ�scene���µ�destroy���������䴦��
        if (!gameObject.scene.isLoaded)
        {
            return;
        }

        //rateDrops��Ӧ���ǲ�һ�����䣬�����ܵ��临�������ߵĴ���ʽ
        //����ÿ��item����һ�����ʣ����÷���ʱ����һ��0-100֮����������ͨ�������õ����ο��Ե����item
        float randomNumber = Random.Range(0f, 100f);
        foreach (Drop rateDrop in rateDrops)
        {
            if (randomNumber <= rateDrop.dropRate)
            {
                DropsInstant(rateDrop);
            }
        }

        //possibleDrops��Ӧһ������䣬����ֻ��������һ����Ʒ�ķ�ʽ
        //���Զ���Ϊ�����͵ĳ�Ա���������ⲿ��ֵ��Ҳ�����������������ɾֲ�����������ش���Ϊ��addԪ��
        //ͨ�����������Щitem��ȡ��һ������
        List<Drop> possibleDrops = new List<Drop>();
        if (possibleDrops.Count >0)
        {
            Drop possibleDrop = possibleDrops[Random.Range(0, possibleDrops.Count)];
            DropsInstant(possibleDrop);
        }
    }

    //�����������ֻ�ڷ�������ִ�У�Ȼ��㲥�����ͻ���
    void DropsInstant(Drop drop)
    {
        if(isServer)
        {
            var temp = Instantiate(drop.itemPrefab, transform.position, Quaternion.identity);
            NetworkServer.Spawn(temp);
        }
    }
}
