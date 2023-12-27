using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBarUI : MonoBehaviour
{
    public PlayerData_SO playerData;

    public int playerMaxHP//ç≈çÇååó 
    {
        get { if (playerData != null) return playerData.playerMaxHP; else return 0; }
    }
    public int playerHP//ìñëOååó 
    {
        get { if (playerData != null) return playerData.playerHP; else return 0; }
    }

    private Image hpBarr;
    // Start is called before the first frame update
    void Start()
    {
        hpBarr = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBarr.fillAmount = (float)playerHP / playerMaxHP;
    }
}
