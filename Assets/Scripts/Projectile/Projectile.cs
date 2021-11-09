using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using MyObjectPools;


public abstract class Projectile : MonoBehaviour
{
    protected ProjectileSO projectileDef;
    protected float damage;
    protected Transform chaTranform;

    protected Vector3 direction;
    protected ObjectPool projPool;
    protected ObjectPool warheadPool;

    protected float lifeTime;

    protected int targetLayer = 8;
    protected int wallLayer = 10;
    protected abstract void OnTriggerEnter2D(Collider2D collision);


    public void defProj(ProjectileSO pso){
        this.projectileDef = pso;
    }

    public void setChaTransform(Transform t){
        chaTranform = t;
        transform.position = t.position;
    }

    public void setDamage(float d){
        damage = d;
    }

    public void setPool(ObjectPool myPool, ObjectPool whPool){
        projPool = myPool;
        warheadPool = whPool;
    }

    public void setProjPool(ObjectPool myPool){
        projPool = myPool;
    }
    public void setWarheadPool(ObjectPool whPool){
        warheadPool = whPool;
    }


    public void setDirect(Vector2 d){
        direction = new Vector3(d.x,d.y,0);
    }

    public Vector2 getDirect(){
        return new Vector2(direction.x,direction.y);
    }

    protected void recycleProjectile(GameObject proj){
        proj.SetActive(false);
        projPool.recycleObject(proj);
    }

    public void resetLifeTime() {
        lifeTime = projectileDef.lifeTime;
    }

    public void setTargetLayer(int lm){
        targetLayer = lm;
    }

    protected void spawnWarhead() {
        GameObject wh = warheadPool.getPooledObject();
        wh.GetComponent<Warhead>().setWarhead(warheadPool);
        wh.transform.position = this.transform.position;
    }



}
