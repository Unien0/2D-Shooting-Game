using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDetailsList_SO", menuName ="Sound/SoundDetailsList")]
public class SoundDetailsList_SO : ScriptableObject
{
    public List<SoundDetails> soundDetailsList;
    public SoundDetails GetSoundDetails(SoundName name)
    {
        //名前で検査
        return soundDetailsList.Find(s => s.soundName == name);
    }
}

[System.Serializable]
public class SoundDetails
{
    public SoundName soundName;
    public AudioClip soundClip;
    [Range(0.1f, 1.5f)]
    public float soundPitchMin;
    [Range(0.1f, 1.5f)]
    public float soundPitchMax;
    [Range(0.1f, 1f)]
    public float soundVolume;
}

//SoundNameの種類（ただ名前を入力しないのもの
public enum SoundName
{
    none,
    Button1, Button2, Button3,
    //按钮1，按钮2，按钮3
    Slash, Shoot,
    //挥砍，射击
    MusicCalm, MenuMusic,
    //BGM1，菜单音乐

    //环境音
    ExperienceGem, HealthPotion,
    //经验球,治疗瓶
    Injured01
    //受伤01
}
