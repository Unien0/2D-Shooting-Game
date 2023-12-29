using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : BasePool<EnemyState>
{
    private PlayerState playerState;

    void Awake()
    {
        Initialized();
    }

    private void Start()
    {

    }

    public void GetExplosion()
    {
        var temp = Get();
        //temp.transform.position = pos;
        //temp.transform.rotation = rotation;
        //temp.GetComponent<Rigidbody2D>().velocity = BulletSpeed * transform.right;

    }

    public void ReleaseExplosion(EnemyState obj)
    {
        Release(obj);
    }
}
