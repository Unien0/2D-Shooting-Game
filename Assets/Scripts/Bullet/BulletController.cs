﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public PlayerBullet_SO playerBulletData;
    public float bulletCoolDownTime//CD
    {
        get { if (playerBulletData != null) return playerBulletData.bulletCoolDownTime; else return 0; }
    }
    public int magazineBulletCount//弹匣子弹量
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
    private float fillingTime;//填装计时器
    [SerializeField][ReadOnly]
    private bool isLoadBullets;//强制装填状态
    [SerializeField][ReadOnly]
    private bool isFillingTime;

    //public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireThreshold = 0.5f;//死区

    private BulletPool bulletPool;

    private void Start()
    {
        currentMagazineBulletCount = magazineBulletCount;//弹匣装满
        bulletPool = FindObjectOfType<BulletPool>();
    }

    void Update()
    {
        bulletTime += Time.deltaTime;
        MousePosition();//鼠标平面坐标

         BulletInstantiate();
         //InputPosition();
            
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

        //按下鼠标左键，如果松开一段时间则判定子弹是否装满，没装满则填装
        if (Input.GetMouseButton(0) )
        {
            //条件：子弹数大于0，且子弹发射CD转好，且不处于强制填装状态
            if (currentMagazineBulletCount > 0  && !isLoadBullets)
            {
                if ( bulletTime >= bulletCoolDownTime)
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
            else if(currentMagazineBulletCount == 0)
            {
                //否则强行装填子弹
                isFillingTime = true;
                LoadBullets();
                //isLoadBullets = true;//启动强制状态状态
                //！！！！！！此处有bug
            }
        }//如果不处于强制填装状态，且子弹数少于弹匣容量，并且在过了一段时间后就会进入填装状态
        else if(currentMagazineBulletCount < magazineBulletCount)
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

    /// <summary>
    /// 手柄射击模式
    /// </summary>
    void InputPosition()
    {
        
        float horizontalInput = Input.GetAxis("RightStickHorizontal");
        float verticalInput = Input.GetAxis("RightStickVertical");

        if (Mathf.Abs(horizontalInput) > fireThreshold || Mathf.Abs(verticalInput) > fireThreshold)//设置死区（在一定值范围内不会射击，防止误触）
        {
            //条件：子弹数大于0，且子弹发射CD转好，且不处于强制填装状态
            if (currentMagazineBulletCount >0 && bulletTime >= bulletCoolDownTime && !isLoadBullets)
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
            else if(currentMagazineBulletCount == 0)
            {
                isFillingTime = true;
                //否则强行装填子弹
                LoadBullets();
                //isLoadBullets = true;//启动强制状态状态
            }
        }
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
    }

    //强制填装，在填装结束后退出强制填装状态
    void LoadBullets()
    {
        if (isFillingTime)
        {
            fillingTime += Time.deltaTime;
            if (fillingTime >= bulletFillingTime)
            {
                currentMagazineBulletCount = magazineBulletCount;
                fillingTime = 0;
                isLoadBullets = false;
                isFillingTime = false;
            }
        }
    }
}
