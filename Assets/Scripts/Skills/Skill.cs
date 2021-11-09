using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;



public abstract class Skill : MonoBehaviour
{
    protected AudioSource attackAudioSource;
    protected SkillSO skillDef;
    public float timer;
    protected Transform chaTranform;
    protected ObjectPool projectilePool;
    protected ObjectPool warheadPool;

    protected abstract bool checkCondition();
    public abstract void launchProjectile();


    public void Awake()
    {
        attackAudioSource = gameObject.AddComponent<AudioSource>();
    }


    public void defSkill(SkillSO skillDef){
        this.skillDef = skillDef;
    }
    public void setChaTransform(Transform t){
        chaTranform = t;
    }
    public void setPool(){
        string poolName = skillDef.projectileDef.projectileName;
        int amount = skillDef.projectileDef.initPoolingAmount;
        GameObject pfb = skillDef.projectileDef.projectilePrefeb;
        projectilePool = GameObject.Find("PoolManager").GetComponent<ObjectPoolManager>().addNewObjPool(pfb,poolName,amount);

        poolName = skillDef.warheadDef.warheadName;
        amount = skillDef.warheadDef.initPoolingAmount;
        pfb = skillDef.warheadDef.warheadPrefeb;
        warheadPool = GameObject.Find("PoolManager").GetComponent<ObjectPoolManager>().addNewObjPool(pfb,poolName,amount);
    }

    public void PlayAttackAudio()
    {
        attackAudioSource.Play();
    }

    protected Projectile createProjectile(){
        GameObject proj_GameObj = projectilePool.getPooledObject();

        Projectile p = proj_GameObj.GetComponent<Projectile>();
        p.setPool(projectilePool, warheadPool);
        p.defProj(skillDef.projectileDef);
        p.setChaTransform(chaTranform);
        p.setDamage(skillDef.damage);
        p.resetLifeTime();
        p.setTargetLayer(8);// Enemies' Layer
        proj_GameObj.SetActive(true);

        return p;
    }

    protected Vector2 rot(Vector2 v, float radius){
        return new Vector2(v.x * Mathf.Cos(radius) + v.y * Mathf.Sin(radius) , v.y * Mathf.Cos(radius) - v.x * Mathf.Sin(radius));
    }

}

