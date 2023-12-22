using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetPlayerControl : NetworkBehaviour
{
    public float MoveSpeed = 5f;
    public bool isDead = false;

    private Vector2 movement;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            LookAt();
        }
    }
    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        rigidbody.velocity = movement.normalized * MoveSpeed;
    }

    void LookAt()
    {
        // Flip sprite if needed
        //var flipSprite = spriteRenderer.flipX ? movement.x > 0.01f : movement.x < -0.01f;
        //if (flipSprite)
        //{
        //    spriteRenderer.flipX = !spriteRenderer.flipX;
        //}

        if(movement.x > 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (movement.x < -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

}
