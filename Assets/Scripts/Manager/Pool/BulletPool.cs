using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : BasePool<Bullet>
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

    public void ReleaseExplosion(Bullet obj)
    {
        Release(obj);
    }
}
