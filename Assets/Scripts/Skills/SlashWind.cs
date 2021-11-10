using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWind : Skill
{
    public AudioClip attackSound;

    private float cooldown;

    private Animator chaAnim;
    private ChaState chaState;
    private BoxCollider2D chaBox;
    private bool storming = false;

    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        attackAudioSource.clip = attackSound;
        chaAnim = chaTranform.GetComponent<ChaAction>().anim;
        chaState = chaTranform.GetComponent<ChaState>();
        chaBox = chaTranform.GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (timer < cooldown && !storming){
            timer += Time.deltaTime;
        }
    }

    protected override bool checkCondition(){
        if (timer >= cooldown && !storming){
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

            
            SwordStorm p = (SwordStorm)createProjectile();
            p.setDirect(direction);
            StartCoroutine(Storming(p));
            
            PlayAttackAudio();
        }
    }

    IEnumerator Storming(SwordStorm p){
        chaState.speed -= 1.5f;
        storming = true;
        yield return new WaitForSeconds(10.0f);
        p.closeStorm();
        storming = false;
        chaState.speed += 1.5f;
    }
}
