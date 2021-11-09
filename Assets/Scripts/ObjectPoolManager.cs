using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Simple Object Pool, reference from https://learn.unity.com/tutorial/introduction-to-object-pooling
*/
namespace MyObjectPools
{

public class ObjectPoolManager : MonoBehaviour
{
    private Dictionary<string, ObjectPool> pools;

    void Awake()
    {
        pools = new Dictionary<string, ObjectPool>();
    }

    public ObjectPool addNewObjPool(GameObject prefeb, string prefebName, int amount){
        if(pools.ContainsKey(prefebName)){
            return pools[prefebName];
        }else{
            amount = (amount>0)? amount:1;
            ObjectPool p = new ObjectPool(prefeb,amount);
            pools.Add(prefebName, p);
            return pools[prefebName];
        }
    }

    public GameObject getPooledObject(string prefebName){
        if(pools.ContainsKey(prefebName)){
            return pools[prefebName].getPooledObject();
        }else{
            Debug.Log(prefebName + "Do not have!");
            return null;
        }
    }

    public ObjectPool getObjectPoolByName(string objName){
        return pools[objName];
    }
    
    /*
    public void recycleObject(string prefebName, GameObject g){
        if(pools.ContainsKey(prefebName)){
            pools[prefebName].recycleObject(g);
        }
    }
    */

}

public class ObjectPool : Object{
    
    public Queue<GameObject> pool;
    public GameObject prefeb;

    public ObjectPool(GameObject prefeb, int amount)
    {
        this.prefeb = prefeb;
        pool = new Queue<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amount; i++){
            tmp = Instantiate(prefeb);
            tmp.SetActive(false);
            pool.Enqueue(tmp);
        }
    }

    public void recycleObject(GameObject g){
        g.SetActive(false);
        pool.Enqueue(g);
        //Debug.Log("RE " + pool.Count);
    }

    public GameObject getPooledObject(){
        GameObject ret;
        if(pool.Count != 0){
            ret = pool.Dequeue();
        }else{
            ret = Instantiate(prefeb);
        }
        ret.SetActive(true);
        //Debug.Log("GT " + pool.Count);
        return ret;
    }

    public int objCount(){
        return this.pool.Count;
    }
}


}
