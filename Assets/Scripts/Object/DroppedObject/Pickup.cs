using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pickup : NetworkBehaviour
{
    //防止重复拾取的第一层flag
    public  bool hasBeenCollected = false;
    //2.15孟：父类的collect方法只改变已拾取的flag，详细变化在各个子物体的collect方法中重写
    public virtual void Collect()
    {
        hasBeenCollected = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ServerKill();
        }
    }

    //掉落物是一次接触就会销毁的Object，因此只需要在服务器上判定，并对其进行服务器销毁
    void ServerKill()
    {
        if (isServer)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
    //在被销毁时，调用collect方法
    private void OnDestroy()
    {
        Collect();
    }
}
