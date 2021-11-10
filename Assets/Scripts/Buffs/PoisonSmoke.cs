using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;

public class PoisonSmoke : Buff
{
    private bool isPoisoning = false;

    void Update() {
        if(victim && victim.activeInHierarchy){
            gameObject.transform.position = victim.transform.position;
        }
        if(!isPoisoning){
            StartCoroutine(Burning());
            isPoisoning = true;
        }
    }


    public override void setBuffAttrs(GameObject v, float d, ObjectPool bp) {
        base.setBuffAttrs(v,d,bp);
        
    }

    IEnumerator Burning()
    {
        float damageDuration = 0.25f; // hard code here. 
        float damage = 1.0f;
        float remainTime = duration;
        while(remainTime > 0){
            if(victim && victim.activeInHierarchy){
                if (victim.layer == Usefuls.UsefulConstant.EnemyLayer){
                    victim.GetComponent<EnemyHealth>().loseHp(damage);
                }
            }else{
                remainTime = -1f;
                break;
            }
            yield return new WaitForSeconds(damageDuration); 
            remainTime -= damageDuration;
        }
        yield return new WaitForSeconds(0.3f); 
        isPoisoning = false;
        recycleBuff();
    }



}
