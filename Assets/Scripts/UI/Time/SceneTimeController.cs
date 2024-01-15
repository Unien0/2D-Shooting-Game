using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class SceneTimeController : MonoBehaviour
{
    private TMP_Text timeText;
    private float sceneStartTime;

    void Start()
    {
        timeText = gameObject.GetComponent<TMP_Text>();
        //场景开始时时间归零
        sceneStartTime = 0f;
    }

    void Update()
    {
        //增加时间
        sceneStartTime += Time.deltaTime;
        // 更新UI Text的文本内容，显示经过的时间
        if (timeText != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(sceneStartTime);
            string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            timeText.text = formattedTime;
        }
    }
}
