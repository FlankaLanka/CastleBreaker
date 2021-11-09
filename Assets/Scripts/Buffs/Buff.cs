using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;


public abstract class Buff : MonoBehaviour
{
    protected GameObject victim;
    protected float duration;
    protected ObjectPool buffPool;


    public virtual void setBuffAttrs(GameObject v, float d, ObjectPool bp){
        victim = v;
        duration = d;
        buffPool = bp;
    }

    protected void recycleBuff(){
        buffPool.recycleObject(gameObject);
    }
}
