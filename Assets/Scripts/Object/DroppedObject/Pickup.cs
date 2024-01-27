using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //拾取道具的基本效果
    public  bool hasBeenCollected = false;
    public virtual void Collect()
    {
        hasBeenCollected = true;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {

            Destroy(gameObject);
        }
    }
}
