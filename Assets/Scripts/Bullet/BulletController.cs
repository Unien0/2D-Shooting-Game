using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;

    void Update()
    {
      
        MousePosition();
        BulletInstantiate();
    }

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

}
