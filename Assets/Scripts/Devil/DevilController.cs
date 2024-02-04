using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔王化控制
/// </summary>
public class DevilController : MonoBehaviour
{
    //思路：
    //在游戏时间进行到一定程度的时候会根据当前的排名来确定谁成为魔王，成为魔王后各项属性提升、血量回满
    //在魔王化后，最大血量随着时间减少，减少速度会越来越快，在击杀敌人后最大血量提高，在最大血量加值为0时魔王化自动取消
    //在一定时间的时候魔王化会强制取消
    //在魔王时间里，其他玩家死亡不会有任何惩罚，但是依然会受到伤害
    //魔王在击杀玩家后将会获得赏金。玩家击杀魔王后会获得三倍赏金（赏金是直接增加还是掉落形式还有待争议）

    //做法，通过广播的形式将魔王后的属性修正传给各个脚本

    public bool demonization;//是否魔王化
    PlayerState playerState;

    void Start()
    {
        //获取组件：判断是否是排名最高的
        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (playerState.highestPoint && GameManager.Instance.devilOpen)
        {
            demonization = true;
            EventCenter.Broadcast<bool>(EventType.Demonization,demonization);//呼叫程序,传入bool值
        }
    }
}
