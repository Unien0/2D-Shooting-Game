using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class PlayerLevel : NetworkBehaviour
{
    [Header("经验值/等级")]
    public int experience = 0;
    public int level = 1;
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
    private Image minExpBar;
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
        minExpBar = GameObject.FindGameObjectWithTag("minExpBar").GetComponent<Image>();

        //首先初始化一次经验条
        UpdateExpBar();
    }

    void Update()
    {
        //UpdateExpBar();
        UpdateLevelText();
    }

    public void IncreaseExperience(int amount)
    {
        IncreaseExpCRPC(amount);

        LevelUpChecker();

        UpdateExpBar();
    }

    [ClientRpc]
    void IncreaseExpCRPC(int amount)
    {
        if(isServer)
        {
            experience += amount;
        }
    }

    void LevelUpChecker()//升级鉴定
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;

                    break;
                }
            }
            experienceCap += experienceCapIncrease;
            UpdateLevelText();
            //游戏状态调整（暂时没做）
            //GameManager.instance.StartLevelUp();

        }
    }

    /// <summary>
    /// 清空经验值
    /// </summary>
    void ExpClear()
    {
        experience = 0;
    }

    //============UI部分==================
    void UpdateExpBar()//经验值UI显示
    {
        expBar.fillAmount = (float)experience / experienceCap;
        minExpBar.fillAmount = (float)experience / experienceCap;
        expText.text = experience.ToString() + " / " + experienceCap.ToString();
    }

    void UpdateLevelText()//等级UI显示
    {
        levelText.text = level.ToString();
    }


}
