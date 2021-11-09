using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallLauncher : Skill
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

            Vector2 v = chaTranform.GetComponentInParent<Rigidbody2D>().velocity;
            Vector2 direction = new Vector2(v.x, v.y);
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
            }
            Projectile p = createProjectile();
            p.setDirect(direction);

            PlayAttackAudio();
        }
    }
}
