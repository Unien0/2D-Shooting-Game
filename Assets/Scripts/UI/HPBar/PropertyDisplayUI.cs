using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 属性显示UI
/// </summary>
public class PropertyDisplayUI : MonoBehaviour
{
    public PlayerData_SO playerData;
    public PlayerBullet_SO playerBullet;

    #region SO数据获取
    public float playerCurrentSpeed//速度
    {
        get { if (playerData != null) return playerData.playerCurrentSpeed; else return 0; }
    }
    //public float playerMaxSpeed//最大速度
    //{
    //    get { if (playerData != null) return playerData.playerMaxSpeed; else return 0; }
    //}
    public float playerBaseSpeed//基础速度
    {
        get { if (playerData != null) return playerData.playerBaseSpeed; else return 0; }
    }
    public int playerBulletDamage//力量
    {
        get { if (playerData != null) return playerBullet.playerBulletDamage; else return 0; }
    }
    public float bulletCoolDownTime//CD
    {
        get { if (playerData != null) return playerBullet.bulletCoolDownTime; else return 0; }
    }
    public float bulletExistenceTime//子弹有效时间
    {
        get { if (playerData != null) return playerBullet.bulletExistenceTime; else return 0; }
    }
    public int playerLucky//最高血量
    {
        get { if (playerData != null) return playerData.playerLucky; else return 0; }
    }
    #endregion

    public TMP_Text speedText;
    public TMP_Text damageText;
    public TMP_Text CDText;
    public TMP_Text bulletExistenceTimeText;
    public TMP_Text luckyText;

    public Color minColor = Color.white;
    public Color maxColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        //UpdateSpeedUI();
    }
    void UpdateUI()
    {
        // Update the UI text values with the corresponding data
        damageText.text = playerBulletDamage.ToString("F1");
        CDText.text = bulletCoolDownTime.ToString("F1");
        bulletExistenceTimeText.text = bulletExistenceTime.ToString("F1");
        luckyText.text = playerLucky.ToString("F1");
    }
    //void UpdateSpeedUI()
    //{
    //    //显示速度数值
    //    speedText.text = playerCurrentSpeed.ToString("F1");
    //    // 根据速度值计算颜色
    //    if (playerCurrentSpeed <= playerBaseSpeed)
    //    {
    //        speedText.color = minColor;  // 当速度小于等于一定值时颜色为白色
    //    }
    //    else
    //    {
    //        float t = (playerCurrentSpeed - playerBaseSpeed) / (playerMaxSpeed - playerBaseSpeed);
    //        speedText.color = Color.Lerp(minColor, maxColor, t);
    //    }
    //}
}
