using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyObjectPools;
using UnityEngine.Assertions;


public class FoxSpark : Buff
{

    //private Rigidbody2D victimRb;
    private Animator victimAnim;



    public override void setBuffAttrs(GameObject v, float d, ObjectPool bp) {
        base.setBuffAttrs(v,d,bp);
        
        
        //victimRb  = victim.GetComponent<Rigidbody2D>();
        //float x = victimRb.velocity.x;
        //float y = victimRb.velocity.y;



        victimAnim = victim.GetComponent<Animator>();
        float x = victimAnim.GetFloat("Look X");
        float y = victimAnim.GetFloat("Look Y");


        // Reset rotation and set it to new direction. 
        //Vector3 q = Quaternion.FromToRotation(gameObject.transform.eulerAngles,new Vector3(0,0,0)).eulerAngles;
        //gameObject.transform.Rotate(q);
        gameObject.transform.eulerAngles = new Vector3 (0,0,0);

        //q = Quaternion.FromToRotation(new Vector3(0,0,0), new Vector3(x,y,0)).eulerAngles;


        Vector3 q = Quaternion.FromToRotation(new Vector3(0,1,0), new Vector3(x,y,0)).eulerAngles;
        gameObject.transform.Rotate(q);

        gameObject.transform.position = victim.transform.position;
    }

    void Start() {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void Update() {
        gameObject.transform.position = victim.transform.position;
    }




    private void OnParticleSystemStopped() {
        recycleBuff();
    }

    private void OnParticleCollision(GameObject other) {
    }


}
