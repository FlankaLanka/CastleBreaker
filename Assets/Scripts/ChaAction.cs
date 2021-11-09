using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaAction : MonoBehaviour
{
    
    public Animator anim;
    private Rigidbody2D rb;
    private ChaState chaState;
    private AudioSource runSound;
    private bool audioIsPlaying = false;

    private bool isAIControled = true;

    int animSpeedTrigger = 0;

    private GameObject moveStick;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        chaState = GetComponent<ChaState>();
        moveStick = GameObject.Find("Fixed Joystick");
        runSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Do Somethings for anim
        if(!chaState.blockMoving && !chaState.blockAnim){
            animSpeedTrigger = 0;
            if (rb.velocity.x != 0 || rb.velocity.y != 0) {
                animSpeedTrigger = 1;
            }
            anim.SetFloat("Speed", animSpeedTrigger);

            if(moveStick.GetComponent<Joystick>().Horizontal == 1 || moveStick.GetComponent<Joystick>().Horizontal == -1
                || moveStick.GetComponent<Joystick>().Vertical == 1 || moveStick.GetComponent<Joystick>().Vertical == -1)
            {
                anim.SetFloat("Look X", rb.velocity.x);
                anim.SetFloat("Look Y", rb.velocity.y);
                if(!audioIsPlaying)
                {
                    audioIsPlaying = true;
                    runSound.Play();
                }
            }
            else
            {
                audioIsPlaying = false;
                runSound.Stop();
            }
        }
    }

    public void switchControlToPlayer()
    {
        isAIControled = false;
    }

    public void switchControlToAI()
    {
        isAIControled = true;
    }

    public void move(float movementX, float movementY){
        if(!chaState.blockMoving){
            float speed = chaState.speed;
            rb.velocity = new Vector2(movementX * speed, movementY * speed);
        }
    }

    public void stop(){
        if(!chaState.blockMoving){
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void changeAIState(){

    }


}
