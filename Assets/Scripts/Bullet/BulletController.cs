using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireThreshold = 0.5f;

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
            // 计算子弹的方向
            Vector2 bulletDirection = new Vector2(horizontalInput, verticalInput).normalized;

            // 创建子弹实例
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 设置子弹的速度和方向
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;

        }

    }

}
