using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup,Icollectible
{
    [Header("经验值道具")]
    public int experienceGranted;

    public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerState player = FindObjectOfType<PlayerState>();
        //player.IncreaseExperience(experienceGranted);
        //EventHandler.CallPlaySoundEvent(SoundName.ExperienceGem);
        //Destroy(gameObject);
    }
    
}
