using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;
using UnityEngine.Assertions;

public class Ignite : Buff
{
    private bool isBurning = false;

    void Update() {
        if(victim && victim.activeInHierarchy){
            gameObject.transform.position = victim.transform.position;
        }
        if(!isBurning){
            StartCoroutine(Burning());
            isBurning = true;
        }
    }


    public override void setBuffAttrs(GameObject v, float d, ObjectPool bp) {
        base.setBuffAttrs(v,d,bp);
        
    }

    IEnumerator Burning()
    {
        float damageDuration = 0.25f; // hard code here. 
        float damage = 5f;
        float remainTime = duration;
        while(remainTime > 0){
            if(victim && victim.activeInHierarchy){
                if (victim.tag == "Enemy" || victim.tag == "EnemyTower" || victim.tag == "Guard_Enemy"){
                    Assert.IsNotNull(victim.GetComponent<EnemyHealth>());
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
        isBurning = false;
        recycleBuff();
    }



}
