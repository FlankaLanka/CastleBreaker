using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCircle : Projectile
{
    // Start is called before the first frame update
    private float projSpeed;
    private int remainHit;

    private float angularVelocity; 


    // relative angular between start|end direction and cha's face direction. 
    private static float relativeAngular = 1.31f;// pi*(5/12)
    private static float radius = 1.25f; 
    private float startAngular;
    private float endAngular;
    private float angularMovement;
    private TrailRenderer trailRenderer;

    void Start()
    {
        projSpeed = projectileDef.projSpeed;
        remainHit = projectileDef.maxHit;

        angularVelocity = projSpeed/radius;
        trailRenderer = GetComponent<TrailRenderer>();
    }

    new public void setDirect(Vector2 d){
        direction = new Vector3(d.x,d.y,0);

        float da = Mathf.Atan2(d.y,d.x);
        if(da >= 0){
            startAngular = da - relativeAngular;
            endAngular = da + relativeAngular;
        }else{
            da += 2*Mathf.PI;
            startAngular = da - relativeAngular;
            endAngular = da + relativeAngular;
        }
        angularMovement = startAngular;
        transform.position = chaTranform.position + 
        new Vector3(radius*Mathf.Cos(angularMovement), radius*Mathf.Sin(angularMovement), 0);
        if(trailRenderer == null){
            trailRenderer = GetComponent<TrailRenderer>();
        }
        trailRenderer.emitting = true;
    }
    // Update is called once per frame
    //void FixedUpdate() {    }

    private bool recycling = false;
    void Update()
    {
        if(!recycling){
            if (angularMovement > endAngular){
                recycling = true;
                StartCoroutine(waitingRecycle());
            }else{
                angularMovement += angularVelocity * Time.deltaTime;
                transform.position = chaTranform.position + new Vector3(radius*Mathf.Cos(angularMovement), radius*Mathf.Sin(angularMovement), 0);
            }
        }
    }


    IEnumerator waitingRecycle(){
        yield return new WaitForSeconds(0.3f);
        trailRenderer.emitting = false;
        angularMovement = startAngular;
        remainHit = projectileDef.maxHit;
        recycling = false;
        recycleProjectile(gameObject);
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
