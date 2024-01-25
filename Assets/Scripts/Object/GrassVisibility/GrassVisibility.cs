using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]//保证有SpriteRenderer
public class GrassVisibility : MonoBehaviour
{
    private Collider2D cl2D;
    private Color originalColor;

    private void Start()
    {
        cl2D = gameObject.GetComponent<Collider2D>();
        originalColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }



}
