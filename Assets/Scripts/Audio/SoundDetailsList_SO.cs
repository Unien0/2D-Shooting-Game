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
    Slash,Shoot,
    //チョップ，しゃげき
    MusicCalm,
    //BGM1
    Square, Triangle, Rhombus, Star,
    //魔法SE
    EnemyAttack1, EnemyAttack2, EnemyAttack3,
    //敵の行動
    Explosion1, Explosion12,
}
