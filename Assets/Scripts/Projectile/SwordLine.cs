using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usefuls;
public class SwordLine : Projectile
{
    // Start is called before the first frame update
    private float projSpeed;
    private int remainHit;

    private Transform smallSword; 

    void Start()
    {
        projSpeed = projectileDef.projSpeed;
        remainHit = projectileDef.maxHit;
        lifeTime = projectileDef.lifeTime;
        smallSword = transform.GetChild(0);
    }

    new public void setDirect(Vector2 d){
        direction = new Vector3(d.x, d.y, 0);
        gameObject.transform.eulerAngles = new Vector3 (0,0,0);
        Vector3 q = Quaternion.FromToRotation(new Vector3(0,1,0), new Vector3(d.x,d.y,0)).eulerAngles;
        gameObject.transform.Rotate(q);
        gameObject.transform.position = chaTranform.position;
    }
    void Update()
    {
        lifeTime -= Time.deltaTime;
        transform.position = chaTranform.position + Vector3.Normalize(direction) * projSpeed * (projectileDef.lifeTime-lifeTime);
        if (lifeTime < 0){
            remainHit = projectileDef.maxHit;
            recycleProjectile(gameObject);
        }

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(remainHit > 0){
            if (collision.gameObject.layer == targetLayer)
            {
                if(targetLayer == 8){
                    if(collision.gameObject.GetComponent<EnemyHealth>() != null){
                        collision.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
                    }
                }else if(targetLayer == 9){
                    if(collision.gameObject.GetComponent<PlayerHealth>() != null){
                        collision.gameObject.GetComponent<PlayerHealth>().loseHp(damage);
                    }
                }
                remainHit--;
            }else if (collision.gameObject.layer == wallLayer){
                remainHit = 0;
            }
        }
    }

}
