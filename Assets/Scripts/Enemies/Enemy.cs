using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;
using Usefuls;

public abstract class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemySO basicInf; // Need to setup in editor. 
    protected string enemyName; 
    protected float hitPoint;
    protected float speed;
    protected float attack;
    protected List<ObjectPool> projPools;
    // Enemies can only care about limited player characters. 
    protected GameObject[] playersList;
    protected Rigidbody2D rb;
    protected EnemyHealth hps;

    protected virtual void Start() {
        projPools = new List<ObjectPool>();
        playersList = GameObject.FindGameObjectsWithTag("Player");
        enemyName = basicInf.enemyName;
        hitPoint = basicInf.hitPoint;
        speed = basicInf.speed;
        attack = basicInf.attack;
        rb = GetComponent<Rigidbody2D>();
        hps = GetComponent<EnemyHealth>();

        // set useful projs for this unit. 
        string poolName;
        int amount;
        GameObject pfb;

        foreach (ProjectileSO pso in basicInf.weaponProjs) {
            poolName = pso.projectileName;
            amount = pso.initPoolingAmount;
            pfb = pso.projectilePrefeb;
            projPools.Add(GameObject.Find("PoolManager").GetComponent<ObjectPoolManager>().addNewObjPool(pfb,poolName,amount));
        }
    }
    protected Projectile createWeaponProj(int index){
        ObjectPool projPool = projPools[index];
        GameObject proj_GameObj = projPool.getPooledObject();
        Projectile p = proj_GameObj.GetComponent<Projectile>();
        p.setPool(projPool,null);
        p.defProj(basicInf.weaponProjs[index]);
        p.setChaTransform(transform);
        p.setDamage(attack);
        p.resetLifeTime();
        p.setTargetLayer(9);// player's Layer
        proj_GameObj.SetActive(true);
        return p;
    }

    protected abstract void strike_1(Transform target);
    protected abstract void strike_2(Transform target);


    protected Transform findInvadePlayer(float guardRange) {
        foreach (GameObject p in playersList) {
            if (checkDistance(p.transform,guardRange)) {
                return p.transform;
            }
        }
        return null;
    }

    protected Transform findNearestInvadePlayer(float guardRange) {
        float dis;
        float cp = Mathf.Infinity;
        float r = guardRange*guardRange;
        Transform ret = null;

        foreach (GameObject p in playersList) {
            dis = computeDistance(p.transform,guardRange);
            if (dis < r) {
                if(dis < cp){
                    if (isVisible(p.transform)){
                        ret = p.transform;
                        cp = dis;
                    }
                }
            }
        }
        return ret;
    }

    protected bool isVisible(Transform t){
        // Check if cloak. 
        if (t.gameObject.layer == UsefulConstant.Hidden){
            return false;
        }
        // Chekc if block by walls. 
        bool ret;
        RaycastHit2D hitedWALL = Physics2D.Linecast(transform.position, t.position, UsefulConstant.WallLayerMask);
        if (hitedWALL && hitedWALL.collider != null){
            ret = false;
        }else{
            ret = true;
        }
        return ret;
    }

    protected bool checkDistance(Transform t, float d){
        return (t.position - transform.position).sqrMagnitude < (d*d);
    }
    protected float computeDistance(Transform t, float d){
        return (t.position - transform.position).sqrMagnitude;
    }

    protected void MoveToward(Vector2 direction){
        rb.velocity = speed * (direction.normalized);
    }

    protected void StopMovement(){
        rb.velocity = new Vector2(0,0);
    }

    protected bool isHurt(){
        return hps.isHurt();
    }
}
