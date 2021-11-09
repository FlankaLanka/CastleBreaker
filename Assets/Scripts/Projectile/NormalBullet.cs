using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class NormalBullet : Projectile
{

    private float projSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = chaTranform.position;
        lifeTime = projectileDef.lifeTime;
        projSpeed = projectileDef.projSpeed;
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
        //if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyTower" || collision.gameObject.tag == "Guard_Enemy")
        if (collision.gameObject.layer == targetLayer)
        {
            if(targetLayer == 8){
                Assert.IsNotNull(collision.gameObject.GetComponent<EnemyHealth>());
                collision.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
            }else if(targetLayer == 9){
                Assert.IsNotNull(collision.gameObject.GetComponent<PlayerHealth>());
                collision.gameObject.GetComponent<PlayerHealth>().loseHp(damage);
            }



            recycleProjectile(gameObject);
        }else if (collision.gameObject.layer == wallLayer){
            recycleProjectile(gameObject);
        }

    }

}
