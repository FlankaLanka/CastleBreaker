using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public class FireBall : Projectile
{
    private float projSpeed;

    private BuffManager bm; 

    // Start is called before the first frame update
    void Start()
    {
        transform.position = chaTranform.position;
        lifeTime = projectileDef.lifeTime;
        projSpeed = projectileDef.projSpeed;
        bm = BuffManager.Instance; 
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyTower" || collision.gameObject.tag == "Guard_Enemy" || collision.gameObject.tag == "Walls"){


        if(collision.gameObject.layer == targetLayer || collision.gameObject.layer == wallLayer){
            Vector2 pos = new Vector2(transform.position.x,transform.position.y);
            Collider2D[] collisions = Physics2D.OverlapCircleAll(pos, 2, 1<<targetLayer);
            foreach (Collider2D c in collisions){
                //if (c.gameObject.tag == "Enemy" || c.gameObject.tag == "EnemyTower" || c.gameObject.tag == "Guard_Enemy"){
                Assert.IsNotNull(c.gameObject.GetComponent<EnemyHealth>());
                c.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
                bm.addBuffTo("Ignite", c.gameObject);
                //}
            }
            spawnWarhead();
            recycleProjectile(gameObject);
            //StartCoroutine(explosion());
        }
    }

}
