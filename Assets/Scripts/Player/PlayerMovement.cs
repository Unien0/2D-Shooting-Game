using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float MoveSpeed = 5f;
    public bool isDead = false;

    private Vector2 movement;
    private new Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        if (!isDead)
        {
            LookAt();
        }
    }
    public void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        rigidbody.MovePosition(rigidbody.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);
    }


    void LookAt()
    {
        movement.x = Input.GetAxis("Horizontal"); 
        movement.y = Input.GetAxis("Vertical");

        // Flip sprite if needed
        if (movement.x > 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (movement.x < -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
