using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Dash : Skill
{
    private float cooldown;

    private ChaAction chaAct;
    private Rigidbody2D rb;
    private ChaState chaState;
    private Animator anim;

    private bool isDashing;
    private bool dashFromIdle;

    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        isDashing = false;
        dashFromIdle = false;
        chaAct = chaTranform.GetComponentInParent<ChaAction>();
        rb = chaTranform.GetComponentInParent<Rigidbody2D>();
        chaState = chaTranform.GetComponentInParent<ChaState>();
        anim = chaTranform.GetComponentInParent<Animator>();
    }

    private void Update() {
        if (timer < cooldown && !isDashing){
            timer += Time.deltaTime;
        }
    }

    protected override bool checkCondition(){
        if (timer >= cooldown && !isDashing){
            timer = 0f;
            return true;
        }
        return false;
    }

    public override void launchProjectile(){
        if(checkCondition()){
            // TODO 
            isDashing = true;
            anim.SetTrigger("Launch");
            StartCoroutine(PlayerDash());
        }
    }





    IEnumerator PlayerDash()
    {
        chaState.blockMoving = true;
        chaState.blockDamage = true;
        chaTranform.gameObject.layer = Usefuls.UsefulConstant.Dashing;

        if (rb.velocity.x == 0 && rb.velocity.y == 0) {
            float x = chaTranform.GetComponent<ChaAction>().anim.GetFloat("Look X");
            float y = chaTranform.GetComponent<ChaAction>().anim.GetFloat("Look Y");
            if (x == 0)
            {
                rb.velocity = new Vector2(0, y);
            }
            else
            {
                rb.velocity = new Vector2(x, 0);
            }

            dashFromIdle = true;
        }

        rb.velocity *= 2.5f;

        StartCoroutine(DashShoot(rb.velocity.x, rb.velocity.y));

        yield return new WaitForSeconds(0.45f); //0.45 sec of dash
        rb.velocity /= 2.5f;
        if (dashFromIdle){
            rb.velocity = new Vector2(0,0);
        }
        dashFromIdle = false;
        isDashing = false;

        chaTranform.gameObject.layer = Usefuls.UsefulConstant.PlayerLayer;

        chaState.blockMoving = false;
        chaState.blockDamage = false;
    }


    IEnumerator DashShoot(float vx, float vy)
    {
        int shootingNum = 6;
        Vector2 direction;

        int enemyLayer = 8; // hard code here. 

        while (shootingNum > 0)
        {
            Collider2D collision = Physics2D.OverlapCircle(chaTranform.position, 6.0f, (1<<enemyLayer));
            // Assert.IsNotNull(collision);
            if(collision != null){
                //if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyTower" || collision.gameObject.tag == "Guard_Enemy"){
                Vector3 d = chaTranform.position - collision.GetComponentInParent<Transform>().position;
                vx = d.x;
                vy = d.y;
                //}
            }
            
            direction = new Vector2(-vx,-vy);

            Projectile p = createProjectile();
            p.setDirect(direction);

            yield return new WaitForSeconds(0.08f); //0.45 sec of dash
            shootingNum--;
        }
    }



}


