using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Skill
{
    public AudioClip attackSound;

    private float cooldown;

    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        attackAudioSource.clip = attackSound;
    }

    private void Update() {
        if (timer < cooldown){
            timer += Time.deltaTime;
        }
    }

    protected override bool checkCondition(){
        if (timer >= cooldown){
            timer = 0f;
            return true;
        }
        return false;
    }

    public override void launchProjectile(){
        if(checkCondition()){

            //Vector2 v = chaTranform.GetComponentInParent<Rigidbody2D>().velocity;
            //Vector2 direction = new Vector2(v.x, v.y);

            float x = chaTranform.GetComponent<ChaAction>().anim.GetFloat("Look X");
            float y = chaTranform.GetComponent<ChaAction>().anim.GetFloat("Look Y");
            Vector2 direction = new Vector2(x, y);

            if (x == 0 && y == 0)
            {
                direction = new Vector2(1, 0);
            }


            float r = -(Mathf.PI/12f);
            for(int i = 0; i < 7; i ++){
                Projectile p = createProjectile();
                p.setDirect(rot(direction,r));
                r += Mathf.PI/36f;
            }

            PlayAttackAudio();
        }
    }

}
