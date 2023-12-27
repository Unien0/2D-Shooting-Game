using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData_SO", menuName = "Data/Player")]
public class PlayerData_SO : ScriptableObject
{
    [Header("PlayerData")]
    [Header("基本资料")]
    public string playerName;//名称
    public Sprite playerIcon;//图标
    public Sprite playerOnWorldSprite;//游戏内画像
    [Multiline] public string playerDescription;//介绍
    [Space(10)]

    [Header("玩家属性")]
    public int playerMaxHP;//最大血量
    public int playerHP;//当前血量
    public float playerReplyTime;//血量回复时间
    public int playerReplyVolume;//回复量
    public int playerLucky;
    [Header("_移动速度相关")]
    public float playerBaseSpeed;//基础速度
    public float playerCurrentSpeed;//当前速度
     [Space(10)]

    [Header("独立开关")]
    public bool isDead;//是否死亡
    public bool moveable;//是否可以移动
    public bool controllable;//是否可以控制

}
