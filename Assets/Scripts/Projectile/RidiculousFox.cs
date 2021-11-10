using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RidiculousFox : Projectile
{
    private float maxHealth; //must set this in editor
    private float currentHealth;

    public Slider HpBar;
    public GameObject FoxFace;
    private bool rotatingLeft = true;
    private float angularVelocity = 60.0f;
    private float angularMovement = 0.0f;
    private float oneSecTimer = 0.0f;

    private void Start()
    {
        //float hitPoint = 
        maxHealth = (float)projectileDef.maxHit;
        currentHealth = maxHealth;
        HpBar.maxValue = maxHealth;
        HpBar.value = currentHealth;
    }

    private void recycleFox(){  
        currentHealth = maxHealth;
        HpBar.value = maxHealth;
        recycleProjectile(gameObject);
    }

    private void Update() {
        if(currentHealth<0){
            recycleFox();
        }else{
            if(oneSecTimer > 1.0f){
                oneSecTimer = 0.0f;
                currentHealth-=1.0f;
                HpBar.value = currentHealth;
            }else{
                oneSecTimer+=Time.deltaTime;
            }
        }
        if(rotatingLeft){
            if(angularMovement < 30.0f){
                FoxFace.transform.eulerAngles += new Vector3(0,0,angularVelocity * Time.deltaTime);
                angularMovement += angularVelocity * Time.deltaTime;
            }else{
                rotatingLeft = false;
            }
        }else{
            if(angularMovement > -30.0f){
                FoxFace.transform.eulerAngles -= new Vector3(0,0,angularVelocity * Time.deltaTime);
                angularMovement -= angularVelocity * Time.deltaTime;
            }else{
                rotatingLeft = true;
            }
        }
    }

    // Update is called once per frame
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == targetLayer)
        {
            if(targetLayer == 8){
                currentHealth-=1.0f;
                HpBar.value = currentHealth;
            }
        }
    }
}
