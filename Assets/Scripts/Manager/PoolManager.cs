using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//对象池
public class PoolManager : MonoBehaviour
{
    //对象池内的物体
    public List<GameObject> poolPrefabs;
    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();
    //声音堆栈
    private Queue<GameObject> soundQueue = new Queue<GameObject>();

    private void OnEnable()
    {
        EventHandler.InitSoundEffect += InitSoundEffect;
    }
    private void OnDisable()
    {
        EventHandler.InitSoundEffect -= InitSoundEffect;
    }

    /// <summary>
    /// 生成对应物体
    /// </summary>
    private void CreateSoundPool()
    {
        var parent = new GameObject(poolPrefabs[0].name).transform;
        parent.SetParent(transform);
        //设置20个空物体
        for (int i = 0; i < 20; i++)
        {
            GameObject newObj = Instantiate(poolPrefabs[0], parent);
            newObj.SetActive(false);
            soundQueue.Enqueue(newObj);
        }
    }
    /// <summary>
    /// 获取音频
    /// </summary>
    /// <returns></returns>
    private GameObject GetPoolObject()
    {
        if (soundQueue.Count < 2)
            CreateSoundPool();
        return soundQueue.Dequeue();
    }

    /// <summary>
    /// 声音初始化
    /// </summary>
    /// <param name="soundDetails"></param>
    private void InitSoundEffect(SoundDetails soundDetails)
    {
        var obj = GetPoolObject();
        obj.GetComponent<Sound>().SetSound(soundDetails);
        obj.SetActive(true);
        StartCoroutine(DisableSound(obj, soundDetails.soundClip.length));
    }

    /// <summary>
    /// 关闭声音播放
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator DisableSound(GameObject obj,float duration)
    {
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);
        soundQueue.Enqueue(obj);
    }

    /// <summary>
    /// 如果需要播放声音的话填写以下内容
    /// </summary>
    private void CallMusic()
    {
        EventHandler.CallPlaySoundEvent(SoundName.none);
    }
}
