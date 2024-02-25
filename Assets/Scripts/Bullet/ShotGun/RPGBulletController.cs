using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RPGBulletController : NetworkBehaviour
{
    [Header("SO获取")]
    public PlayerBullet_SO playerBulletData;
    public DevilData_SO devilData;

    private float bulletCoolDownTime//CD
    {
        get { if (playerBulletData != null) return playerBulletData.bulletCoolDownTime; else return 0; }
    }
    private int magazineBulletCount//弹匣子弹量
    {
        get { if (playerBulletData != null) return playerBulletData.magazineBulletCount; else return 0; }
    }
    private float bulletFillingTime//填装时间
    {
        get { if (playerBulletData != null) return playerBulletData.bulletFillingTime; else return 0; }
    }
    private float bulletLoadingStartTime//填装启动时间
    {
        get { if (playerBulletData != null) return playerBulletData.bulletLoadingStartTime; else return 0; }
    }
    [Header("只读参数")]
    [SerializeField]
    [ReadOnly]
    private float bulletTime;//当前射击冷却
    [SerializeField]
    [ReadOnly]
    private int currentMaxMagazineBulletCount;//当前弹匣最高子弹量
    [SerializeField]
    [ReadOnly]
    private int currentMagazineBulletCount;//当前子弹量
    [SerializeField]
    [ReadOnly]
    private float fillingTimeCD;//当前装填冷却时间，不会进入状态
    [SerializeField]
    [ReadOnly]
    private float fillingTime;//填装计时器
    [SerializeField]
    [ReadOnly]
    private bool isLoadBullets;//强制装填状态
    [SerializeField]
    [ReadOnly]
    private bool isFillingTime;

    //public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireThreshold = 0.5f;//死区

    [Header("UI")]
    public Image bulletBarDown;//换弹UI的背景
    public Image bulletLoadingDisplay;//换弹进度显示
    public TMP_Text bulletCountDisplay;//子弹数量显示
    public Image bulletCountIcon;//子弹数量的Icon

    private Color initColor = new Color(1, 1, 1, 0f);//换弹UI的初始透明度（α通道）为0，即平时不显示
    private Color targetColor = new Color(1, 1, 1, 1f);//换弹UI的初始透明度（α通道）为1，即换挡时显示

    private BulletPool bulletPool;
    private DevilController devilController;
    public PlayerState playerState;
    public Transform firePos;
    //1.14孟：现行版本，因无法同时兼容对象池的正常使用，因此暂时将对象池化的子弹改为传统的生成·销毁方法
    public GameObject bulletPrefab;
    public GameObject devilBulletPrefab;

    private void Awake()
    {
        EventCenter.AddListener<bool>(EventType.Demonization, BecomingDevil);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventType.Demonization, BecomingDevil);
    }

    private void Start()
    {
        currentMaxMagazineBulletCount = magazineBulletCount;
        currentMagazineBulletCount = currentMaxMagazineBulletCount;//弹匣装满
        bulletPool = FindObjectOfType<BulletPool>();
        playerState = GetComponentInParent<PlayerState>();//获取父物体上的组件
        devilController = GetComponentInParent<DevilController>();

        //判断只有当前客户端的本地玩家，才显示子弹数量和提示图标
        if (isLocalPlayer)
        {
            bulletCountDisplay.enabled = true;
            bulletCountIcon.enabled = true;
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        ChangeRotation();
        BulletMouseSync();
        //MousePosition();//鼠标平面坐标
        //InputPosition();
        UIBulletCount();//子弹数量显示
    }

    private void BecomingDevil(bool demonization)
    {

    }



    /// <summary>
    /// 鼠标定位
    /// </summary>
    void ChangeRotation()
    {
        //获得鼠标的实际坐标
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //找到鼠标和玩家之间的夹角，并转化为角度角
        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;

        //改变枪gameobject的角度
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    [Command]
    void Init()
    {
        // 实例化子弹并设置位置和旋转
        if (!devilController.demonization)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, transform.rotation);
            NetworkServer.Spawn(bullet);
        }
        else
        {
            GameObject bullet = Instantiate(devilBulletPrefab, firePos.position, transform.rotation);
            NetworkServer.Spawn(bullet);
        }


    }
    /// <summary>
    /// 鼠标射击模式
    /// 2024.1.22 孟：给网络通信的子弹发射方法加上注释，免得找不到
    /// 2024.02.13 孟：完成了子弹UI同步显示和换弹UI同步显示
    /// </summary>
    void BulletMouseSync()
    {
        //按下鼠标左键，射击
        if (Input.GetMouseButton(0) && !isLoadBullets)
        {
            fillingTime = 0;
            bulletTime += Time.deltaTime;

            if (bulletTime >= bulletCoolDownTime)
            {
                Init();
                bulletTime -= bulletCoolDownTime;
                currentMagazineBulletCount--;

            }
        }
        if (currentMagazineBulletCount >= currentMaxMagazineBulletCount)
        {
            fillingTimeCD = 0;
        }

        if (currentMagazineBulletCount <= 0)
        {
            isLoadBullets = true;
            isFillingTime = true;
            LoadBullets();
        }
    }

    //强制填装，在填装结束后退出强制填装状态
    void LoadBullets()
    {
        //图标显示
        bulletBarDown.DOColor(targetColor, 0.25f);
        if (isFillingTime)
        {
            fillingTime += Time.deltaTime;
            //UI变化
            float fillAmount = fillingTime / bulletFillingTime;
            bulletLoadingDisplay.fillAmount = fillAmount;

            if (fillingTime >= bulletFillingTime)
            {
                currentMagazineBulletCount = currentMaxMagazineBulletCount;
                fillingTime = 0;
                isFillingTime = false;
                isLoadBullets = false;
                //换弹结束后，换弹UI槽归零，同时UI不显示
                bulletLoadingDisplay.fillAmount = 0;
                bulletBarDown.DOColor(initColor, 0.25f);
            }
        }
    }
    void UIBulletCount()
    {
        if (bulletCountDisplay.enabled)
        {
            bulletCountDisplay.text = currentMagazineBulletCount + "/" + currentMaxMagazineBulletCount.ToString();
        }
    }
    /// <summary>
    /// 鼠标射击模式
    /// 2024.1.11 孟：修改了换弹逻辑，满足设计需求
    /// </summary>
    void BulletMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //按下鼠标左键，如果松开一段时间则判定子弹是否装满，没装满则填装
        if (Input.GetMouseButton(0) && !isLoadBullets)
        {
            fillingTimeCD = 0;  //只要进入射击状态，就会打断脱战自动装填的CD,以及装填进度
            fillingTime = 0;
            bulletTime += Time.deltaTime;
            if (bulletTime >= bulletCoolDownTime)
            {
                // 实例化子弹并设置位置和旋转
                var bullet = bulletPool.Get();
                bullet.transform.position = this.transform.position;
                bullet.transform.rotation = this.transform.rotation;
                bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.right;
                bulletTime -= bulletCoolDownTime;//CD还原
                currentMagazineBulletCount--;//子弹数量减少
            }
        }
        //不按左键，视为脱战状态，如果此时子弹数少于弹匣容量，经过一段时间后自动装填
        else if (currentMagazineBulletCount < currentMaxMagazineBulletCount)
        {
            //计时器，如果fillingTimeCD 大于等于填装启动时间，则进入填装状态，此时如果发射子弹，则计时归0
            fillingTimeCD += Time.deltaTime;
            if (fillingTimeCD >= bulletLoadingStartTime)
            {
                //由于不是强制填装状态，所以玩家可以随时退出填装
                isFillingTime = true;
                LoadBullets();
            }
        }
        //当前子弹数大于等于弹匣容量时，可判定换弹完成，计时器也清零
        if (currentMagazineBulletCount >= currentMaxMagazineBulletCount)
        {
            fillingTimeCD = 0;
        }
        //子弹数量为0时，强制进入换弹状态，且此时无法攻击
        if (currentMagazineBulletCount == 0)
        {
            isLoadBullets = true;//启动强制状态状态
            isFillingTime = true;
            LoadBullets();
        }
    }

    /// <summary>
    /// 手柄射击模式
    /// </summary>
    void BulletGamePad()
    {

        float horizontalInput = Input.GetAxis("RightStickHorizontal");
        float verticalInput = Input.GetAxis("RightStickVertical");

        if (Mathf.Abs(horizontalInput) > fireThreshold || Mathf.Abs(verticalInput) > fireThreshold)//设置死区（在一定值范围内不会射击，防止误触）
        {
            //条件：子弹数大于0，且子弹发射CD转好，且不处于强制填装状态
            if (currentMagazineBulletCount > 0 && bulletTime >= bulletCoolDownTime && !isLoadBullets)
            {
                //子弹大于0时生成子弹
                // 计算子弹的方向
                Vector2 bulletDirection = new Vector2(horizontalInput, verticalInput).normalized;

                // 创建子弹实例
                //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                var bulletInput = bulletPool.Get();
                bulletInput.transform.position = this.transform.position;
                bulletInput.transform.rotation = Quaternion.identity;

                // 设置子弹的速度和方向
                bulletInput.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
                bulletTime -= bulletCoolDownTime;//CD还原
                currentMagazineBulletCount--;
            }
            else if (currentMagazineBulletCount == 0)
            {
                isFillingTime = true;
                //否则强行装填子弹
                LoadBullets();
                //isLoadBullets = true;//启动强制状态状态
            }
        }
        else if (currentMagazineBulletCount < currentMaxMagazineBulletCount)
        {
            //计时器，如果fillingTimeCD 大于等于填装启动时间，则进入填装状态
            fillingTimeCD += Time.deltaTime;
            if (fillingTimeCD >= bulletLoadingStartTime)
            {
                //由于不是强制填装状态，所以玩家可以随时退出填装
                isFillingTime = true;
                LoadBullets();
                fillingTimeCD = 0;
            }
        }
    }



    /// 2024.1.11旧版本的键鼠操作模式下的子弹模式代码
    /// 已弃用，仅做存档，最终版本确定后删除
    /*void BulletInstantiate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //按下鼠标左键，如果松开一段时间则判定子弹是否装满，没装满则填装
        if (Input.GetMouseButton(0))
        {
            //条件：子弹数大于0，且子弹发射CD转好，且不处于强制填装状态
            if (currentMagazineBulletCount > 0 && !isLoadBullets)
            {

                if (bulletTime >= bulletCoolDownTime)
                {
                    // 实例化子弹并设置位置和旋转
                    var bullet = bulletPool.Get();
                    bullet.transform.position = this.transform.position;
                    bullet.transform.rotation = this.transform.rotation;
                    bullet.GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.right;
                    bulletTime = 0;//CD还原
                    currentMagazineBulletCount--;//子弹数量减少
                }
            }
            else if (currentMagazineBulletCount == 0)
            {
                //否则强行装填子弹
                isFillingTime = true;
                LoadBullets();
                //isLoadBullets = true;//启动强制状态状态
                //！！！！！！此处有bug
            }
        }//如果不处于强制填装状态，且子弹数少于弹匣容量，并且在过了一段时间后就会进入填装状态
        else if (currentMagazineBulletCount < magazineBulletCount)
        {
            //计时器，如果fillingTimeCD 大于等于填装启动时间，则进入填装状态
            fillingTimeCD += Time.deltaTime;
            if (fillingTimeCD >= bulletLoadingStartTime)
            {
                //由于不是强制填装状态，所以玩家可以随时退出填装
                isFillingTime = true;
                LoadBullets();
                fillingTimeCD = 0;
            }
        }
    }*/
}
