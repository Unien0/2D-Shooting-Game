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

    [Header("显示获胜玩家UI")]
    private PlayerDiedCheck[] players;
    private string winner;
    public GameObject winnerUI;
    public TMP_Text winnerName;

    private double runTime;
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

    private void Start()
    {
        FindObjectOfType<CustomNetworkRoomManager>().showBackRoomBtm = false;
    }
    private void Update()
    {
        runTime = GameManager.Instance.gameTime;
        //在服务器上，每脕E丒馔婕沂?
        if (isServer && runTime >= 5)
        {
            players = FindObjectsOfType<PlayerDiedCheck>();
            Debug.Log("目前玩家数量：" + players.Length);

            //当玩家仅剩一人时，即游戏结蕘E
            if (players.Length == 1)
            {
                //找到那仅剩的玩家，显示在UI上
                foreach (PlayerDiedCheck player in players)
                {
                    int playerId = player.gameObject.GetComponentInParent<PlayerState>().playerId;
                    winner = "P" + (1 + playerId).ToString();
                }

                winnerUI.SetActive(true);
                winnerName.text = winner;
                FindObjectOfType<CustomNetworkRoomManager>().showBackRoomBtm = true;

                ShowWinnerGUICRPC(winner);
            }
        }
    }

    [ClientRpc]
    void ShowWinnerGUICRPC(string name)
    {
        winnerUI.SetActive(true);
        winnerName.text = name;
        FindObjectOfType<CustomNetworkRoomManager>().showBackRoomBtm = true;
    }

    [ClientRpc]
    public void InitRank(string playerId) //新玩家加葋E词弊远跗诨桓雠琶丒
    {
        rankList[number].GetComponent<TextMeshProUGUI>().text = playerId;
        number++;
    }


    public void AddScore(string playerId, int socre)
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
        switch (rank)
        {
            case 1:
                if (rankList[0].activeInHierarchy)
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
