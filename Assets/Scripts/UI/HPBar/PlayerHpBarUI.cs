using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBarUI : MonoBehaviour
{
    private Image hpBar;
    // Start is called before the first frame update
    void Start()
    {
        hpBar = gameObject.GetComponent<Image>();
    }

    public void UpdateHpBar(int playerHp,int playerMaxHp)
    {
        hpBar.fillAmount = (float)playerHp / playerMaxHp;
    }
}
