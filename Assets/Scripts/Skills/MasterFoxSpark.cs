using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MasterFoxSpark : Skill
{
    private float cooldown;

    private Rigidbody2D rb;
    private ChaState chaState;
    private Animator anim;

    private bool isFiring;

    private BuffManager bm; 


    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        isFiring = false;
        rb = chaTranform.GetComponentInParent<Rigidbody2D>();
        anim = chaTranform.GetComponentInParent<Animator>();
        chaState = chaTranform.GetComponentInParent<ChaState>();
        bm = BuffManager.Instance; 
    }


    private void Update() {
        if (timer < cooldown && !isFiring){
            timer += Time.deltaTime;
        }
    }

    protected override bool checkCondition(){
        if (timer >= cooldown && !isFiring){
            timer = 0f;
            return true;
        }
        return false;
    }

    public override void launchProjectile(){
        if(checkCondition()){
            // TODO 
            //isFiring = true;
            StartCoroutine(Firing());
        }
    }



    IEnumerator Firing()
    {
        isFiring = true;
        chaState.blockAnim = true;
        chaState.speed = 0.5f;
        anim.SetTrigger("Launch");

        float remainTime = 11.0f; // hard code here. 
        float damageDuration = 0.25f; // hard code here. 
        float damage = skillDef.damage;

        int enemyLayer = 8;
        //int wallLayer = 10;// hard code here, 8 is enemies layer, while 10 is wall layer. 

        //int enemylayerMask = (1<<enemyLayer) | (1<<wallLayer); 

        //float x = rb.velocity.x;
        //float y = rb.velocity.y;
        float x = anim.GetFloat("Look X");
        float y = anim.GetFloat("Look Y");
        
        bm.addBuffTo("FoxSpark", chaTranform.gameObject);


        Vector2 dd;
        if(x == 0 && y == 0){
            dd = new Vector2(0, 1);
        }else{
            dd = new Vector2(x,y);//Vector2(rb.velocity.x, rb.velocity.y);
        }
        dd.Normalize();
        dd *= 3.0f;


        Vector2 startPos = new Vector2(chaTranform.position.x, chaTranform.position.y);
        startPos+=dd;
        
        
        Collider2D c;
        while(remainTime > 0){
            startPos = new Vector2(chaTranform.position.x, chaTranform.position.y)+dd;
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll(startPos, 4.0f, dd, 7.0f, (1<<enemyLayer));
            foreach (RaycastHit2D rayHit in rayHits){
                c = rayHit.collider;
                //if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "EnemyTower" || c.gameObject.tag == "Guard_Enemy"){
                Assert.IsNotNull(c.gameObject.GetComponent<EnemyHealth>());
                c.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
                //}
            }
            yield return new WaitForSeconds(damageDuration); 
            remainTime -= damageDuration;
        }

        isFiring = false;
        chaState.blockAnim = false;
        chaState.speed = 1.5f;
    }



}


