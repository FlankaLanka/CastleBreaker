using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Projectile
{

    private BuffManager bm; 

    // Start is called before the first frame update
    void Start()
    {
        transform.position = chaTranform.position;
        bm = BuffManager.Instance; 
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 3, 1<<targetLayer);

            if(targetLayer == 8){
                foreach (Collider2D c in collisions){
                    c.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
                    bm.addBuffTo("Poison", c.gameObject);
                }
            }else if(targetLayer == 9){
                foreach (Collider2D c in collisions){
                    c.gameObject.GetComponent<PlayerHealth>().loseHp(damage);
                    bm.addBuffTo("Poison", c.gameObject);
                }
            }
            spawnWarhead();
            recycleProjectile(gameObject);
        }
    }

}
