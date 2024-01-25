using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]//保证有SpriteRenderer
public class GrassVisibility : MonoBehaviour
{
    private Collider2D cl2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cl2D =GetComponent<Collider2D>();
    }

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FadeOut();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FadeIn();
        }
    }

    /// <summary>
    /// 恢复颜色
    /// </summary>
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        spriteRenderer.DOColor(targetColor, 0.35f);
    }

    /// <summary>
    /// 半透明化
    /// </summary>
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, 0.5f);
        spriteRenderer.DOColor(targetColor, 0.35f);
    }

}
