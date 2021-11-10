using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCircle : Projectile
{
    // Start is called before the first frame update
    private float projSpeed;
    private int remainHit;

    private float angularVelocity; 


    private static float radius = 1.25f; 
    private float endAngular = 160f;
    private float angularMovement;
    private TrailRenderer trailRenderer;

    public GameObject trailGen;
    void Start()
    {
        projSpeed = projectileDef.projSpeed;
        remainHit = projectileDef.maxHit;

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
    }

    private bool recycling = false;
    void Update()
    {
        if(!recycling){
            if (angularMovement > endAngular){
                recycling = true;
                StartCoroutine(waitingRecycle());
            }else{
                angularMovement += angularVelocity * Time.deltaTime;
                transform.eulerAngles += new Vector3(0,0,angularVelocity * Time.deltaTime);
                transform.position = chaTranform.position;
            }
        }
    }


    IEnumerator waitingRecycle(){
        yield return new WaitForSeconds(0.3f);
        trailRenderer.emitting = false;
        angularMovement = 0f;
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
