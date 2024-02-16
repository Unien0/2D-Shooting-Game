using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pickup : NetworkBehaviour
{
    //防止重复拾取的第一层flag
    public  bool hasBeenCollected = false;
    //2.15孟：父类的collect方法只改变已拾取的flag，详细变化在各个子物体的collect方法中重写
    public virtual void Collect(GameObject temp)
    {
        hasBeenCollected = true;
    }

}
