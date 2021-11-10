using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStorm : Projectile
{
    // Start is called before the first frame update
    private float projSpeed;
    private float angularVelocity; 


    private static float radius = 1.25f; 
    private float angularMovement;
    private TrailRenderer trailRenderer;

    public GameObject trailGen;
    void Start()
    {
        projSpeed = projectileDef.projSpeed;

        angularVelocity =  Mathf.Rad2Deg*(projSpeed/radius);
        trailRenderer = trailGen.GetComponent<TrailRenderer>();
    }

    new public void setDirect(Vector2 d){
        direction = new Vector3(d.x,d.y,0);

        gameObject.transform.eulerAngles = new Vector3 (0,0,0);
        Vector3 q = Quaternion.FromToRotation(new Vector3(0,1,0), new Vector3(d.x,d.y,0)).eulerAngles;
        gameObject.transform.Rotate(q);
        gameObject.transform.position = chaTranform.position;

        if(trailRenderer == null){
            trailRenderer = trailGen.GetComponent<TrailRenderer>();
        }
        trailRenderer.emitting = true;
        storming = true;
    }

    private bool storming = false;
    void Update()
    {
        if(storming){
            angularMovement += angularVelocity * Time.deltaTime;
            transform.eulerAngles += new Vector3(0,0,angularVelocity * Time.deltaTime);
            transform.position = chaTranform.position;
        }
    }

    public void closeStorm(){
        trailRenderer.emitting = false;
        angularMovement = 0f;
        storming = false;
        recycleProjectile(gameObject);
    }

    //private Rigidbody2D targetRb;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            if(targetLayer == 8){
                if(collision.gameObject.GetComponent<EnemyHealth>() != null){
                    //targetRb = collision.gameObject.GetComponent<Rigidbody2D>();
                    //targetRb.AddForce(new Vector2(3,0),ForceMode2D.Impulse);
                    collision.gameObject.GetComponent<EnemyHealth>().loseHp(damage);
                    
                }
            }else if(targetLayer == 9){
                if(collision.gameObject.GetComponent<PlayerHealth>() != null){
                    collision.gameObject.GetComponent<PlayerHealth>().loseHp(damage);
                }
            }
        }
    }


}
