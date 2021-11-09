using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : Skill
{
    private float cooldown;

    public SkillSO shotgunSO;
    public SkillSO ap_gunSO;

    private Skill shotgun;
    private Skill ap_gun;
    private Skill hand_gun;
    
    private bool setGun;
    private int currentGun;

    private void Start() {
        cooldown = skillDef.cooldown;
        timer = cooldown;
        setGun = false;
        currentGun = 0;
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
        if(!setGun){
            setUpBagGun();
            setGun = true;
        }

        if(checkCondition()){
            // TODO 
            if (currentGun == 0){
                chaTranform.GetComponentInParent<ChaSkillLauncher>().setSkill(shotgun,0);
                currentGun = 1;
            }else if(currentGun == 1){
                chaTranform.GetComponentInParent<ChaSkillLauncher>().setSkill(ap_gun,0);
                currentGun = 2;
            }else if(currentGun == 2){
                chaTranform.GetComponentInParent<ChaSkillLauncher>().setSkill(hand_gun,0);
                currentGun = 0;
            }
        }
    }

    private void setUpBagGun(){
        hand_gun = chaTranform.GetComponentInParent<ChaSkillLauncher>().getSkillList()[0];

        shotgun = Instantiate(shotgunSO.skillPrefeb, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Skill>();
        shotgun.defSkill(shotgunSO);
        shotgun.setChaTransform(chaTranform);
        shotgun.setPool();

        ap_gun = Instantiate(ap_gunSO.skillPrefeb, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Skill>();
        ap_gun.defSkill(ap_gunSO);
        ap_gun.setChaTransform(chaTranform);
        ap_gun.setPool();

    }
}
