using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBullet_SO", menuName = "Data/Bullet/PlayerBullet")]
public class PlayerBullet_SO : ScriptableObject
{
    [Header("子弹资料")]
    [Header("基础属性")]
    [Header("_伤害")]
    public int playerBulletDamage;//基础力量
    [Header("_子弹冷却时间")]
    public float linerBulletCoolDownTime;//基础
    [Header("_存在时间")]
    public float linerBulletExistenceTime;//存在时间


    [Header("直线子弹")]
    [Header("_速度")]
    public float linerBulletSpeed;//基础
    //public float linerBulletCurrentSpeed;//当前
    [Header("_伤害倍率")]
    public int linerBulletDamageMultipler;//子弹倍率
    //public int linerBulletCurrentDamage;//当前
    [Header("_穿透")]
    public int linerBulletPenetrationCount;//穿透次数


    [Header("追踪子弹")]
    [Header("_速度")]
    public float trackingBulletSpeed;//基础
}
