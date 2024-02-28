using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using TMPro;

public class RankManager : NetworkBehaviour
{
    private static RankManager instance;

    public static RankManager Instance
    {
        get => instance;
    }

    public List<GameObject> rankList;
    private int number = 0;

    private Dictionary<string, int> playerScores = new Dictionary<string, int>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    [ClientRpc]
    public void InitRank(string playerId) //新玩家加葋E词弊远跗诨桓雠琶丒
    {
        rankList[number].GetComponent<TextMeshProUGUI>().text = playerId;
        number++;
    }

    
    public void AddScore(string playerId,int socre)
    {
        playerScores[playerId] = socre;
        UpdateLeaderboard();
    }
    private void UpdateLeaderboard()
    {
        var sortedScores = playerScores.OrderByDescending(x => x.Value);
        int rank = 1;
        foreach (var pair in sortedScores)
        {
            string playerId = pair.Key;
            UpdateLeaderboardUI(rank, playerId);
            rank++;
        }
    }
    [ClientRpc]
    void UpdateLeaderboardUI(int rank, string playerId)
    {
        switch(rank)
        {
            case 1:
                if(rankList[0].activeInHierarchy)
                {
                    rankList[0].GetComponent<TextMeshProUGUI>().text = playerId;
                }
                return;
            case 2:
                if (rankList[1].activeInHierarchy)
                {
                    rankList[1].GetComponent<TextMeshProUGUI>().text = playerId;
                }
                return;
            case 3:
                if (rankList[2].activeInHierarchy)
                {
                    rankList[2].GetComponent<TextMeshProUGUI>().text = playerId;
                }
                return;
            case 4:
                if (rankList[3].activeInHierarchy)
                {
                    rankList[3].GetComponent<TextMeshProUGUI>().text = playerId;
                }
                return;
            default:
                //Debug.Log("出现序列之外的排脕E);
                return;
        }
    }
}
