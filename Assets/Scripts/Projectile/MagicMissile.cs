using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;



public class MagicMissile : Projectile
{
    private float projSpeed;
    private int checkDua;
    private int checkCD;
    private Transform target;
    private bool hasTarget;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = chaTranform.position;
        lifeTime = projectileDef.lifeTime;
        projSpeed = projectileDef.projSpeed;
        hasTarget = false;
        checkDua = 5;
        checkCD = 10;
    }

    private void FixedUpdate() {
        if(hasTarget && target){
            Vector3 tmp = target.position - transform.position;
            direction = new Vector2(tmp.x,tmp.y);
        }else{
            seekAndDestory();
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Vector3.Normalize(direction) * projSpeed * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0){
            recycleProjectile(gameObject);
        }
    }

    private void seekAndDestory(){
        if(checkCD == 0){
            checkCD = checkDua;
            Vector2 pos = new Vector2(transform.position.x,transform.position.y);
            //Collider2D collision = Physics2D.OverlapCircle(pos, 6.0f);
            Collider2D collision = Physics2D.OverlapCircle(pos, 6.0f, 1<<targetLayer);
            //Assert.IsNotNull(collision);
            if(collision != null){
                //if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyTower" || collision.gameObject.tag == "Guard_Enemy"){
                hasTarget = true;
                target = collision.GetComponentInParent<Transform>();
                //}
            }
        }
        checkCD--;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyTower" || collision.gameObject.tag == "Guard_Enemy")
        if (collision.gameObject.layer == targetLayer)
        {
            Assert.IsNotNull(collision.gameObject.GetComponent<EnemyHealth>());
            if(collision.gameObject.GetComponent<EnemyHealth>() != null){
                collision.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
                recycleProjectile(gameObject);
            }
        }
        if (collision.gameObject.layer == wallLayer){
            recycleProjectile(gameObject);
        }
    }
}
