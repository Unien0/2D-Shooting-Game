using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBarUI : MonoBehaviour
{
    public PlayerData_SO playerData;

    public int playerMaxHP;
    public int playerHP;

    private Image hpBarr;
    // Start is called before the first frame update
    void Start()
    {
        ShowHPUI();
        hpBarr = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowHPUI();
        hpBarr.fillAmount = (float)playerHP / playerMaxHP;
    }

    void ShowHPUI()
    {
        playerMaxHP = GetComponentInParent<PlayerState>().currentMaxHp;
        playerHP = GetComponentInParent<PlayerState>().currentHp;
    }
}
