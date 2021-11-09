using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;


public abstract class Warhead : MonoBehaviour
{
    protected ObjectPool warheadPool;
    //protected WarheadSO warheadDef;
    private bool inited = false;

    protected abstract void warheadStart();

    public void setWarhead(ObjectPool whPool){
        if (!inited){
            //this.warheadDef = wso;
            this.warheadPool = whPool;
            inited = true;
        }
    }

    protected void recycleWarhead(){
        warheadPool.recycleObject(gameObject);
    }



//

}
