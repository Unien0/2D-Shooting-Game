using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PlayerLevel : NetworkBehaviour
{
    [Header("经验值/等级")]
    [SyncVar (hook = nameof(UpdateExpBar))]
    public int experience = 0;
    [SyncVar]
    public int level = 1;
    [SyncVar]
    public int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;//启动等级
        public int endLevel;//结束等级，在这一定区间内升级都需要同样的经验值
        public int experienceCapIncrease;//所需经验值（浮动项，根据当前等级改编）
    }

    public List<LevelRange> levelRanges;//等级列表


    [Header("UI")]
    public Image minExpBar;
    private Image expBar;
    private TMP_Text levelText;
    private TMP_Text expText;

    private void Awake()
    {
        EventCenter.AddListener(EventType.isDead,ExpClear);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.isDead, ExpClear);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
        //各个UI显示=>大经验条，大经验条文字，等级，迷你经验条

        expBar = GameObject.FindGameObjectWithTag("ExpBar").GetComponent<Image>();
        expText = GameObject.FindGameObjectWithTag("ExpText").GetComponent<TMP_Text>();
        levelText = GameObject.FindGameObjectWithTag("Level").GetComponent<TMP_Text>();

        //首先初始化一次经验条和等级
        InitExpBar();
        levelText.text = level.ToString();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            levelText.text = level.ToString();
        }
    }

    public void IncreaseExperience(int amount)
    {
        if (isServer)
        {
            experience += amount;

            LevelUpChecker();
        }
    }

    void LevelUpChecker()//升级鉴定
    {
        if (experience >= experienceCap)
        {
            level++;
            int experienceCapIncrease = 0;
            int formerCap = experienceCap;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;

                    break;
                }
            }
            experienceCap += experienceCapIncrease;
            experience -= formerCap;
            GetComponent<PlayerState>().currentMaxHp += 30;
            GetComponent<PlayerState>().currentHp += 30;

            //游戏状态调整（暂时没做）
            //GameManager.instance.StartLevelUp();

        }
    }

    /// <summary>
    /// 清空经验值
    /// </summary>
    [ServerCallback]
    void ExpClear()
    {
        experience = 0;
    }

    //============UI部分==================
    void InitExpBar()//大经验槽，大经验值槽数值，迷你经验槽三者初始化
    {
        if (isLocalPlayer) //大经验槽相关只是显示本地玩家的
        {
            expBar.fillAmount = (float)experience / experienceCap;
            expText.text = experience.ToString() + " / " + experienceCap.ToString();
        }
        minExpBar.fillAmount = (float)experience / experienceCap;
    }
    void UpdateExpBar(int oldExperience, int newExperience)//大经验槽，大经验值槽数值，迷你经验槽三者跟随exp变化改变
    {
        if(isLocalPlayer)
        {
            Debug.Log(experienceCap);
            expBar.fillAmount = (float)experience / experienceCap;
            expText.text = experience.ToString() + " / " + experienceCap.ToString();
        }
        minExpBar.fillAmount = (float)experience / experienceCap;
    }

}
