using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Mirror;

[RequireComponent(typeof(SpriteRenderer))]//保证有SpriteRenderer
public class GrassVisibility : NetworkBehaviour
{
    private Collider2D cl2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cl2D =GetComponent<Collider2D>();
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
