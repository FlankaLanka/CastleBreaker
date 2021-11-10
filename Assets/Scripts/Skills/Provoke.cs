using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Provoke : Skill
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
        if(projectilePool.objCount() <= 0){
            return false;
        }
        if (timer >= cooldown){
            timer = 0f;
            return true;
        }
        return false;
    }

    public override void launchProjectile(){
        if(checkCondition()){
            Projectile p = createProjectile();
            PlayAttackAudio();
        }
    }
}
