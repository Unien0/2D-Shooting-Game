using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ExperienceGem : Pickup
{
    [Header("经验值道具")]
    public int experienceGranted;

    public override void Collect(GameObject temp)
    {
        //防止重复拾取
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            temp.GetComponent<PlayerLevel>().IncreaseExperience(experienceGranted);
            temp.GetComponent<PlayerState>().GetPoint(experienceGranted);
            //EventHandler.CallPlaySoundEvent(SoundName.ExperienceGem);
            //Destroy(gameObject);
        }
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject Obj = col.gameObject;
            //增加点数
            Collect(Obj);

            //掉落物是一次接触就会销毁的Object，因此只需要在服务器上判定，并对其进行服务器销毁
            NetworkServer.Destroy(gameObject);
        }
    }

}
