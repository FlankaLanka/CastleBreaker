using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usefuls;

public class EnemyDefendTowerMovement : Enemy
{
    private WaitForSeconds shotPeriod = new WaitForSeconds(0.125f);
    private int shotNum = 20;
    private Transform target;
    private bool isCooling_1 = false;
    private bool inGuard = false;
    private float guardRange = 8.0f;
    // Update is called once per frame
    private SpriteRenderer SignalLight;


    private Coroutine twAlert;
    protected override void Start() {
        base.Start();
        if(!inGuard){
            twAlert = StartCoroutine(TowerGuard());
            inGuard = true;
        }
        SignalLight = GetComponent<SpriteRenderer>();
    }

    IEnumerator TowerGuard(){
        while (true)
        {
            if(!isCooling_1){
                // Check if have player nearby. 
                target = findNearestInvadePlayer(guardRange);
                if(target != null){
                    StartCoroutine(machineGunShot(target));
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void PowerOff(){
        StopCoroutine(twAlert);
        SignalLight.color = Color.black;
    }
    public void PowerOn(){
        twAlert = StartCoroutine(TowerGuard());
        SignalLight.color = Color.red;
    }

    // Basic Enemies rifle
    protected override void strike_1(Transform target){
        StartCoroutine(machineGunShot(target));
    }

    IEnumerator machineGunShot(Transform t){
        //Debug.Log("Fire!");
        isCooling_1 = true;
        Vector2 direction = t.position - transform.position;
        Projectile p = createWeaponProj(0);
        p.setDirect(direction);
        for(int i = 0; i < shotNum; i++){
            yield return shotPeriod;
            direction = t.position - transform.position;
            p = createWeaponProj(0);
            p.setDirect(UsefulFunction.RotVector2(direction, Random.Range(-Mathf.PI/12f, Mathf.PI/12f)));
        }
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        yield return new WaitForSeconds(5.0f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        isCooling_1 = false;
    }


    protected override void strike_2(Transform target){

    }
}
