using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usefuls;

// EnemyClock who use handGun shot player. 
public class EnemyClockMK2Movement : Enemy
{
    public GameObject SignalLight;
    private WaitForSeconds shotPeriod = new WaitForSeconds(0.125f);
    private int shotNum = 2;

    private enum EnemyState
    {
        SLEEP,
        GUARD,
        HUNTING,
        ATTACK
    }
    private EnemyState currentState = EnemyState.SLEEP;


    private bool SMNotStart = true;
    // Update is called once per frame
    protected override void Start() {
        base.Start();
        if(SMNotStart){
            StartCoroutine(updateState());
            SMNotStart = false;
        }
    }




    IEnumerator updateState(){
        float waitTime;
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.SLEEP:
                    waitTime = doSleepWork();
                    break;
                case EnemyState.GUARD:
                    waitTime = doGuardWork();
                    break;
                case EnemyState.HUNTING:
                    waitTime = doHuntingWork();
                    break;
                case EnemyState.ATTACK:
                    waitTime = doAttackWork();
                    break;
                default:
                    waitTime = 0.2f;
                    break;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Transform target;

    private float doSleepWork(){
        // TODO
        // Check if hited.
        if (isHurt()){
            //Debug.Log("HIT TO GUARD");
            currentState = EnemyState.GUARD;
            SignalLight.GetComponent<SpriteRenderer>().color = Color.blue;
            return 0.01f;
        }
        // Check if have player nearby. 
        target = findInvadePlayer(3.0f);
        if(target != null){
            // Check if player is hidden
            if (target.gameObject.layer == UsefulConstant.Hidden){
                target = null;
                StopMovement();
                return 0.5f;
            }
            // Check if player behind wall
            RaycastHit2D hit = Physics2D.Linecast(transform.position, target.position, UsefulConstant.WallLayerMask);
            if (hit && hit.collider != null){
                target = null;
                StopMovement();
                return 0.5f;
            }else{
                //Debug.Log("SCARE TO GUARD");
                currentState = EnemyState.GUARD;
                SignalLight.GetComponent<SpriteRenderer>().color = Color.blue;
                target = null;
                return 0.01f;
            }
        }else{
            StopMovement();
            return 0.5f;
        }
    }

    private float guardRange = 6.0f;
    private float lostRange = 8.0f;
    private float attackRange = 5.0f;
    private float doGuardWork(){
        target = findInvadePlayer(guardRange);
        if(target != null){
            // do cloak check
            if (target.gameObject.layer == UsefulConstant.Hidden){
                target = null;
                StopMovement();
                return 0.5f;
            }
//            Debug.Log("HUNT!!");
            currentState = EnemyState.HUNTING;
            SignalLight.GetComponent<SpriteRenderer>().color = Color.red;
            return 0.01f;
        }
        StopMovement();
        return 0.5f;
    }

    private float doHuntingWork(){
        if(target.gameObject.layer == UsefulConstant.Hidden || !checkDistance(target,lostRange)){
            currentState = EnemyState.GUARD;
            SignalLight.GetComponent<SpriteRenderer>().color = Color.blue;
            target = null;
            return 1.0f;
        }else if(checkDistance(target,attackRange)){
            StopMovement();
            currentState = EnemyState.ATTACK;
            return 0.2f;
        }else{
            Vector2 direction = target.position - transform.position;
            MoveToward(direction);
            return 0.25f;
        }
    }

    private float doAttackWork(){
        if(!isCooling_1){
            strike_1(target);
            //Vector2 d = transform.position - target.position;
            //MoveToward(UsefulFunction.RotVector2(d, Random.Range(-Mathf.PI/3f, Mathf.PI/3f)));
            currentState = EnemyState.HUNTING;
            return 0.75f;
        }else{
            if(checkDistance(target,3.0f)){
                Vector2 d = transform.position - target.position;
                MoveToward(UsefulFunction.RotVector2(d, Random.Range(-Mathf.PI/6f, Mathf.PI/6f)));
                return 0.5f;
            }else{
                return 0.25f;
            }
        }
    }

    // Basic Enemies rifle
    protected override void strike_1(Transform target){
        StartCoroutine(inaccurateShot(target));
    }

    private bool isCooling_1 = false;
    IEnumerator inaccurateShot(Transform t){
        isCooling_1 = true;
        Vector2 direction = t.position - transform.position;
        Projectile p = createWeaponProj(0);
        p.setDirect(direction);
        for(int i = 0; i < shotNum; i++){
            yield return shotPeriod;
            p = createWeaponProj(0);
            p.setDirect(UsefulFunction.RotVector2(direction, Random.Range(-Mathf.PI/18f, Mathf.PI/18f)));
        }
        Vector2 d = transform.position - target.position;
        MoveToward(UsefulFunction.RotVector2(d, Random.Range(-Mathf.PI/3f, Mathf.PI/3f)));
        yield return new WaitForSeconds(2.0f);
        isCooling_1 = false;
    }


    protected override void strike_2(Transform target){

    }
}
