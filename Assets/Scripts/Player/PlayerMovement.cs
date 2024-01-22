using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
public class PlayerMovement : NetworkBehaviour
{
    public float MoveSpeed = 5f;
    public bool isDead = false;

    private Vector2 movement;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        FindObjectOfType<CinemachineVirtualCamera>().Follow = this.transform;
        if (!isDead)
        {
            //LookAt();
            //PlayerDirection();

        }
    }
    public void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        rigidbody.MovePosition(rigidbody.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// ��1.21����ʱȡ�����ƶ���������ҳ��򣬸�Ϊ����귽�����
    /// </summary>
    void LookAt()
    {

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

    void PlayerDirection()
    {
      
        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 mousePositionScreen = Input.mousePosition;

        // ����Ļ����ת��Ϊ��������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mousePositionScreen);

        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
        Debug.Log(angle);

        CmdFlip(angle);
    }

    [Command]
    void CmdFlip(float angle)
    {
        if (angle > 90f || angle < -90f)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        CRpcFlip(angle);
    }

    [ClientRpc]
    void CRpcFlip(float angle)
    {
        if (angle > 90f || angle < -90f)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
