using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetWorkManager : NetworkManager
{
    private static int nextPlayerId = 0;
    private static Dictionary<int, int> playerNames = new Dictionary<int, int>();

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        nextPlayerId++;
        playerNames[conn.connectionId] = nextPlayerId;

        conn.identity.GetComponent<PlayerState>().playerId = nextPlayerId;
        string playerName = "P" + nextPlayerId.ToString();
        RankManager.Instance.InitRank(playerName);//每加入一个玩家，点亮一个rank

    }

    //[TargetRpc]
    //private void TargetSetPlayerName(NetworkConnection target, string playerName)
    //{
        
    //}
}
