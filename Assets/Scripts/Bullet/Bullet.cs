using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float existenceTime;
    [SerializeField][ReadOnly]
    private float newTime;

    //组件获取
    private Rigidbody2D rb2D;
    private Collider2D col2d;
    private BulletPool parentPool;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        parentPool = FindObjectOfType<BulletPool>();
    }

    // Update is called once per frame
    void Update()
    {
        Existence();
    }

    void Existence()
    {
        newTime += Time.deltaTime;
        if (newTime >= existenceTime)
        {
            newTime = 0f;
            parentPool.ReleaseExplosion(this);
        }
    }
}
