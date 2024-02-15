using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup,Icollectible
{
    [Header("经验值道具")]
    public int experienceGranted;

    public override void Collect()
    {
        //防止重复拾取
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            PlayerLevel player = FindObjectOfType<PlayerLevel>();
            player.IncreaseExperience(experienceGranted);
            PlayerState point = FindObjectOfType<PlayerState>();
            point.GetPoint(experienceGranted);
            //EventHandler.CallPlaySoundEvent(SoundName.ExperienceGem);
            //Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Collect();
    }
}
