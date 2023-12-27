using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public PlayerBullet_SO playerBulletData;
    public float bulletCoolDownTime//CD
    {
        get { if (playerBulletData != null) return playerBulletData.bulletCoolDownTime; else return 0; }
    }
    public float bulletExistenceTime//存在时间
    {
        get { if (playerBulletData != null) return playerBulletData.bulletExistenceTime; else return 0; }
    }
    public int magazineBulletCount//子弹量
    {
        get { if (playerBulletData != null) return playerBulletData.magazineBulletCount; else return 0; }
    }
    public float bulletFillingTime//填装时间
    {
        get { if (playerBulletData != null) return playerBulletData.bulletFillingTime; else return 0; }
    }
    public float bulletLoadingStartTime//填装启动时间
    {
        get { if (playerBulletData != null) return playerBulletData.bulletLoadingStartTime; else return 0; }
    }
    [SerializeField][ReadOnly]
    private float bulletTime;//当前射击冷却
    [SerializeField][ReadOnly]//用于私有只读
    private int currentMagazineBulletCount;//当前子弹量
    [SerializeField][ReadOnly]
    private float fillingTimeCD;//当前装填冷却时间，不会进入状态状态
    [SerializeField][ReadOnly]
    private bool isLoadBullets;//强制装填状态

    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireThreshold = 0.5f;//死区

    private void Start()
    {
        currentMagazineBulletCount = magazineBulletCount;//弹匣装满
    }

    void Update()
    {
        //MousePosition();
        //BulletInstantiate();
        InputPosition();
    }

    /// <summary>
    /// 鼠标定位
    /// </summary>
    void MousePosition()
    {
        // 获取鼠标在屏幕上的位置
        Vector3 mousePositionScreen = Input.mousePosition;

        // 将屏幕坐标转换为世界坐标
        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePositionScreen);

        // 输出世界坐标
        //Debug.Log("Mouse Position in World Coordinates: " + mousePositionWorld);
    }

    void BulletInstantiate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (Input.GetMouseButtonDown(0))
        {
            // 实例化子弹并设置位置和旋转
            FindObjectOfType<BulletPool>().GetExplosion(this.transform.position, transform.rotation, bulletSpeed);
            //GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            //bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.right;
        }
    }

    /// <summary>
    /// 手柄射击模式
    /// </summary>
    void InputPosition()
    {
        float horizontalInput = Input.GetAxis("RightStickHorizontal");
        float verticalInput = Input.GetAxis("RightStickVertical");

        if (Mathf.Abs(horizontalInput) > fireThreshold || Mathf.Abs(verticalInput) > fireThreshold)//设置死区（在一定值范围内不会射击，防止误触）
        {
            bulletTime += Time.deltaTime;
            //条件：子弹数大于0，且子弹发射CD转好，且不处于强制填装状态
            if (currentMagazineBulletCount >0 && bulletTime >= bulletCoolDownTime && !isLoadBullets)
            {
                //子弹大于0时生成子弹
                // 计算子弹的方向
                Vector2 bulletDirection = new Vector2(horizontalInput, verticalInput).normalized;

                // 创建子弹实例
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // 设置子弹的速度和方向
                bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
                currentMagazineBulletCount--;
            }
            else
            {
                //否则强行装填子弹
                LoadBullets();
                isLoadBullets = true;//启动强制状态状态
            }
        }
        else
        {
            if (!isLoadBullets && currentMagazineBulletCount < magazineBulletCount)
            {
                fillingTimeCD += Time.deltaTime;
                if (fillingTimeCD >= bulletLoadingStartTime)
                {
                    LoadBullets();
                    fillingTimeCD = 0;
                }
            }            
        }
    }

    void LoadBullets()
    {
        if (isLoadBullets)//出于装填状态下
        {
            float time = 0f;
            time += Time.deltaTime;
            if (time >= bulletFillingTime)
            {
                currentMagazineBulletCount = magazineBulletCount;
                time = 0;
                isLoadBullets = false;
            }
        }
    }

}
