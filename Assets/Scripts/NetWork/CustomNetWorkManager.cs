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
        RankManager.Instance.InitRank(playerName);//ÿ����һ����ң�����һ��rank

        ResetOldEnemyParent();
    }

    void ResetOldEnemyParent()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int i = 0;
        foreach(GameObject enemy in enemies)
        {
            enemy.transform.SetParent(enemy.GetComponent<EnemyState>().parentTransform);
            Debug.Log("��ʼ��ʱȷʵ�޸���" + i++);
        }
    }

    //[TargetRpc]
    //private void TargetSetPlayerName(NetworkConnection target, string playerName)
    //{
        
    //}
}
