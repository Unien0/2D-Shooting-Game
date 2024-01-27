using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData_SO", menuName = "Data/Enemy")]
public class EnemyData_SO : ScriptableObject
{
    [Header("EnemyData")]
    [Header("基本资料")]
    public int enemyTypesID;
    public string enemyName;
    public Sprite enemyIcon;//图标
    public Sprite enemyOnWorldSprite;//游戏内画像
    [Multiline] public string enemyIntroduction;//敌人介绍

    [Header("基础数值")]
    public int enemyHp;
    public float enemySpeed;
    public int enemyDamage;
    public int enemyPoint;

    [Header("独立开关")]
    public bool isDead;//是否死亡
    public bool moveable;//是否可以移动
    public bool controllable;//是否可以控制


}
