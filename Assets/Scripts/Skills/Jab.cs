using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jab : Skill
{
    public AudioClip attackSound;

    private float cooldown;

    private Animator chaAnim;
    private Sneak SneakSkill;
    private float bounsDamage = 150.0f;

    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        attackAudioSource.clip = attackSound;
        chaAnim = chaTranform.GetComponent<ChaAction>().anim;
        SneakSkill = (Sneak)chaTranform.GetComponent<ChaSkillLauncher>().getSkillList()[2];
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
            SwordLine p = (SwordLine)createProjectile();
            p.setDirect(direction);
            if(chaTranform.gameObject.layer == Usefuls.UsefulConstant.Hidden){
                SneakSkill.leaveSneaking();
                p.setDamage(skillDef.damage+bounsDamage);
            }
            PlayAttackAudio();
        }
    }
}
