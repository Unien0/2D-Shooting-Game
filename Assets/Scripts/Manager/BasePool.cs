using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class BasePool<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected T objPrefab;
    [SerializeField] int defaultSize = 10;
    [SerializeField] int maxSize = 50;
    ObjectPool<T> pool;

    public int ActiveCount => pool.CountActive;
    public int InactiveCount => pool.CountInactive;
    public int TotalCount => pool.CountAll;


    protected void Initialized(bool collectionCheck = true)
    {
        pool = new ObjectPool<T>
                    (createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultSize, maxSize);
    }

    protected virtual T createFunc()
    {
        var obj = Instantiate(objPrefab, transform);
        return obj;
    }

    protected virtual void actionOnGet(T obj)
    {
        obj.gameObject.SetActive(true);
    }
    protected virtual void actionOnRelease(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected virtual void actionOnDestroy(T obj)
    {
        Destroy(obj.gameObject);
    }

    public T Get()
    {
        var t = pool.Get();
        return t;
    }
    public void Release(T obj)
    {
        pool.Release(obj);
    }
    public void Clear()
    {
        pool.Clear();
    }
}

