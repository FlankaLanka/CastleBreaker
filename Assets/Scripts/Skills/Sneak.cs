using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usefuls;

public class Sneak : Skill
{
    public AudioClip attackSound;

    private float cooldown;
    private Jab JabSkill;
    public GameObject fogPrefeb;

    private GameObject fog; 
    private ChaState chaState;
    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        attackAudioSource.clip = attackSound;
        JabSkill = (Jab)chaTranform.GetComponent<ChaSkillLauncher>().getSkillList()[0];
        fog = Instantiate(fogPrefeb);
        fog.SetActive(false);
        chaState = chaTranform.GetComponent<ChaState>();
    }

    private void Update() {
        if (timer < cooldown && !ifSneaking){
            timer += Time.deltaTime;
        }
        if(ifSneaking){
            fog.transform.position = chaTranform.position;
        }
    }

    protected override bool checkCondition(){
        if (timer >= cooldown && !ifSneaking){
            timer = 0f;
            return true;
        }
        return false;
    }

    public override void launchProjectile(){
        if(checkCondition()){
            StartCoroutine(sneaking());            
            PlayAttackAudio();
        }
    }

    private bool ifSneaking = false;
    private WaitForSeconds checkDuration = new WaitForSeconds(0.5f);

    IEnumerator sneaking(){
        ifSneaking = true;
        chaTranform.gameObject.layer = Usefuls.UsefulConstant.Hidden;
        int ctr = 0;
        chaState.speed += 1.5f;
        fog.SetActive(true);

        while (ifSneaking && ctr < 30)
        {
            yield return checkDuration;
            ctr++;
        }

        chaState.speed -= 1.5f;
        chaTranform.gameObject.layer = Usefuls.UsefulConstant.PlayerLayer;
        fog.SetActive(false);
        ifSneaking = false;
    }


    public void leaveSneaking(){
        ifSneaking = false;
    }
}
