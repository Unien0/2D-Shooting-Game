using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DevilData_SO", menuName = "Data/Devil")]
public class DevilData_SO : ScriptableObject
{
    [Header("魔王化")]
    public bool isDevil;
    public float devilTime;//魔王化时间

    [Header("魔王化数值")]
    [Header("血量加成")]
    public int devilMaxHp;//在魔王化后会有一次回血
    [Header("血量自动流失速度（x秒后掉y+1）")]
    public float maxHPLossFrequency;//掉血速度
    public int maxHPlossCount;
    [Header("魔王化后的伤害加成")]
    public int devilDamage;//魔王化后加伤
    [Header("魔王化后的移动速度")]
    public float devilSpeed;//移动速度
    [Header("魔王化后射速提高（当前射击间隔-此射击间隔）")]
    public float devilCoolDownTime;//子弹射击间隔
    [Header("魔王化后填装时间")]
    public float devilbulletFillingTime;//填装时间
    [Header("魔王化后图像放大")]
    public float magnificationRate;//放大倍率（用来改变角色模型大小
    [Header("魔王化后的击杀赏金，魔王被击杀后参与者获得3倍赏金")]
    public int killBounty;//赏金

}
