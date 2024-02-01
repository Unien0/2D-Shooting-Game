using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevel : MonoBehaviour
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
        minExpBar = GameObject.FindGameObjectWithTag("minExpBar").GetComponent<Image>();
        expBar = GameObject.FindGameObjectWithTag("ExpBar").GetComponent<Image>();
        levelText = GameObject.FindGameObjectWithTag("Level").GetComponent<TMP_Text>();
    }

    void Update()
    {
        //UpdateExpBar();
        UpdateLevelText();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();

        UpdateExpBar();
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
    }

    void UpdateLevelText()//等级UI显示
    {
        levelText.text = level.ToString();
    }


}
