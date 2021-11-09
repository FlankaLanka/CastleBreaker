using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Skill
{
    public AudioClip attackSound;

    private float cooldown;

    private Animator chaAnim;

    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        attackAudioSource.clip = attackSound;
        chaAnim = chaTranform.GetComponent<ChaAction>().anim;
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

            float x = chaAnim.GetFloat("Look X");
            float y = chaAnim.GetFloat("Look Y");
            Vector2 direction = new Vector2(x, y);
            if (x == 0 && y == 0)
            {
                direction = new Vector2(1, 0);
            }

            
            SwordCircle p = (SwordCircle)createProjectile();
            p.setDirect(direction);
            
            PlayAttackAudio();
        }
    }
}
