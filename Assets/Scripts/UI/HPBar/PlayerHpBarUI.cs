using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBarUI : MonoBehaviour
{
    private Image hpBar;
    void Awake()
    {
        hpBar = GetComponent<Image>();
    }

    private void Start()
    {
        hpBar.fillAmount = 1;
    }

    public void UpdateHpBar(int playerHp, int playerMaxHp)
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)playerHp / playerMaxHp;

        }
    }
}
