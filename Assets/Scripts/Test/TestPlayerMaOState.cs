using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMaOState : MonoBehaviour
{
    public GameObject gun1;
    public GameObject gun2;


    private Vector3 initialScale;
    private Vector3 targetScale;

    public float scaleChangeSpeed = 1.0f;

    void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale * 2.5f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            // 切换协程，根据当前缩放状态选择逆向缩放或正向缩放
            StartCoroutine(IsScaled() ? ScaleOverTime(initialScale) : ScaleOverTime(targetScale));
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            gun1.SetActive(true);
            gun2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            gun1.SetActive(false);
            gun2.SetActive(false);
        }
    }

    // 判断当前是否处于缩放状态
    private bool IsScaled()
    {
        return transform.localScale == targetScale;
    }

    private System.Collections.IEnumerator ScaleOverTime(Vector3 target)
    {
        float startTime = Time.time;

        while (Time.time - startTime < 1.0f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, (Time.time - startTime) * scaleChangeSpeed);
            yield return null;
        }

        // 确保最终缩放值准确
        transform.localScale = target;
    }
}
