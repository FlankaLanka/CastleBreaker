using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class APBullet : Projectile
{

    private float projSpeed;
    private int remainHit;
    private float remainDamage;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = chaTranform.position;
        lifeTime = projectileDef.lifeTime;
        projSpeed = projectileDef.projSpeed;
        remainHit = projectileDef.maxHit;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Vector3.Normalize(direction) * projSpeed * Time.deltaTime;


        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 || remainHit <= 0){
            remainHit = projectileDef.maxHit;
            recycleProjectile(gameObject);
        }
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyTower" || collision.gameObject.tag == "Guard_Enemy")
        if (collision.gameObject.layer == targetLayer)
        {
            Assert.IsNotNull(collision.gameObject.GetComponent<EnemyHealth>());
            collision.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
            remainHit--;
            damage/=2;
        }
    }

}
