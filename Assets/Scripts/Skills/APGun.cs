using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APGun : Skill
{
    public AudioClip attackSound;
    private float cooldown;
    private ChaState chaState;
    private ChaAction chaAction;



    // I am really dislike this part of code, But I do not have better idea....maybe we chould find other design for APGun. 
    public GameObject scopePrefeb;
    private Joystick moveStick;
    private GameObject switchButton;
    private GameObject ab1;
    private GameObject ab2;
    private Camera playerCamera;
    private bool isScoping = false;
    private GameObject scopeSprite;
    private float cameraOrthoSize;  

    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        attackAudioSource.clip = attackSound;

        // I am really dislike this part of code, But I do not have better idea....maybe we chould find other design for APGun. 
        chaState = chaTranform.GetComponentInParent<ChaState>();
        chaAction = chaTranform.GetComponentInParent<ChaAction>();
        moveStick = GameObject.Find("Fixed Joystick").GetComponent<Joystick>();
        switchButton = GameObject.Find("SwitchCharacter");
        ab1 = GameObject.Find("Ability1");
        ab2 = GameObject.Find("Ability3");
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        scopeSprite = Instantiate(scopePrefeb);
        scopeSprite.SetActive(false);
        cameraOrthoSize = playerCamera.orthographicSize;
    }

    private void Update() {
        if (!isScoping && timer < cooldown){
            timer += Time.deltaTime;
        }
        if(isScoping){
            scopeSprite.transform.position = scopeSprite.transform.position + Vector3.Normalize(new Vector3(moveStick.Horizontal, moveStick.Vertical,0f)) * 4.0f * Time.deltaTime;
        }
    }

    protected override bool checkCondition(){
        if(isScoping){
            return true;
        }else{
            if (timer >= cooldown){
                return true;
            }
        }
        return false;
    }

    public override void launchProjectile(){
        if(checkCondition()){
            if(isScoping){
                shoot();
                timer = 0f;
                scoping(false);
            }else{
                scoping(true);
            }
        }
    }


    private void shoot(){
        //Vector2 v = chaTranform.GetComponentInParent<Rigidbody2D>().velocity;
        Vector3 d = scopeSprite.transform.position - chaTranform.position;
        Vector2 direction = new Vector2(d.x, d.y);
        /*
        if (v.x == 0 && v.y == 0)
        {
            float x = chaTranform.GetComponent<ChaAction>().anim.GetFloat("Look X");
            float y = chaTranform.GetComponent<ChaAction>().anim.GetFloat("Look Y");
            if (x == 0)
            {
                direction = new Vector2(0, y);
            }
            else
            {
                direction = new Vector2(x, 0);
            }
        }*/
        Projectile p = createProjectile();
        p.setDirect(direction);

        PlayAttackAudio();
    }

    private void scoping(bool active){
        if(active){
            chaAction.stop();
            scopeSprite.transform.position = chaTranform.position;
            playerCamera.orthographicSize *= 2.0f;
            
            chaState.blockMoving = true;
            scopeSprite.SetActive(true);
            if(switchButton!= null){
                switchButton.SetActive(false);
            }else{
                switchButton = GameObject.Find("SwitchCharacter");
            }
            ab1.SetActive(false);
            ab2.SetActive(false);
            isScoping = true;
        }else{
            playerCamera.orthographicSize = cameraOrthoSize;
            chaState.blockMoving = false;
            scopeSprite.SetActive(false);
            if(switchButton!= null){
                switchButton.SetActive(true);
            }else{
                switchButton = GameObject.Find("SwitchCharacter");
            }

            ab1.SetActive(true);
            ab2.SetActive(true);
            isScoping = false;
        }
    }

}
