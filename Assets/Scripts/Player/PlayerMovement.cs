using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;
public class PlayerMovement : NetworkBehaviour
{
    public PlayerData_SO playerData;

    private bool isDead;

    public float currentMoveSpeed;

    private Vector2 movement;
    private new Rigidbody2D rigidbody;
    private SpriteRenderer sr;
    private PlayerState playerState;
    private Transform playerTransform;

    //1.30：进入草丛隐藏UI试做
    public Canvas playerUICanvas;

    /*private void Awake()
    {
        EventCenter.AddListener(EventType.isDead, ResurrectionMovement);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.isDead, ResurrectionMovement);
    }*/

    void Start()
    {
        currentMoveSpeed = playerData.playerBaseSpeed;
        playerTransform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        isDead = GetComponent<PlayerState>().isDead;

        FindObjectOfType<CinemachineVirtualCamera>().Follow = this.transform;

        //LookAt();
        //PlayerDirection();

    }
    public void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if (!isDead)//玩家死亡后无法移动
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            //rigidbody.MovePosition(rigidbody.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);
            rigidbody.velocity = movement.normalized * currentMoveSpeed;
        }
    }

    /// <summary>
    /// 玩家朝向
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
      
        // 鼠标位置获取
        Vector3 mousePositionScreen = Input.mousePosition;

        // 鼠标世界坐标
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

    /// <summary>
    /// 重生位置修改,将其传送到随机的一个点上
    /// </summary>
    /*void ResurrectionMovement()
    {
        int PointsCount = GameManager.Instance.relativeSpawnPoints.Count;
        playerTransform.position = GameManager.Instance.relativeSpawnPoints[Random.Range(0, PointsCount)].position;
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Grass"))
        {
            if(isLocalPlayer)
            {
                other.GetComponent<GrassVisibility>().FadeOut();
            }
            else
            {
                playerUICanvas.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            if (isLocalPlayer)
            {
                other.GetComponent<GrassVisibility>().FadeIn();
            }
            else
            {
                playerUICanvas.enabled = true;
            }
        }
    }
}
