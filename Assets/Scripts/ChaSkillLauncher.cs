using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ChaSkillLauncher : MonoBehaviour {

    private ChaState chaState;

    private List<Skill> skills;


    private void Awake() {
        skills = new List<Skill>();
    }
    
    // Start is called before the first frame update
    void Start() {
        chaState = GetComponent<ChaState>();
        int i = 0;
        foreach (SkillSO sso in chaState.skills) {
            Skill skill_obj = Instantiate(sso.skillPrefeb, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Skill>();
            if(skill_obj == null){
                Debug.Log("Catch! "+i);
            }
            i++;
            skill_obj.defSkill(sso);
            skill_obj.setChaTransform(transform);
            skill_obj.setPool();

            skills.Add(skill_obj);
        }
        //Debug.Log("Add Skill "+skills.Count);
        //skills[0].checkCondition();
    }

    public void use_skill_1(){
        skills[0].launchProjectile();
    }

    public void use_skill_2(){
        skills[1].launchProjectile();
    }

    public void use_skill_3(){
        skills[2].launchProjectile();
    }

    public void use_item(){ }

    public List<Skill> getSkillList(){
        return this.skills;
    }

    public void setSkill(Skill s, int index){
        skills[index] = s;
    }

}

/*

    public int weaponType = 0;

    public void switchControlToPlayer()
    {
        isAIControled = false;
        if (weaponType == 0){
            projectile.GetComponent<ProjectileFly>().isAIControled = false;
        }else{
            projectile.GetComponent<ProjectileSeek>().isAIControled = false;
        }
    }

    public void switchControlToAI()
    {
        isAIControled = true;
        if (weaponType == 0){
            projectile.GetComponent<ProjectileFly>().isAIControled = true;
        }else{
            projectile.GetComponent<ProjectileSeek>().isAIControled = true;
        }
    }

*/


/*
    // Update is called once per frame
    void Update()
    {
        if (isAIControled || rof_count > 0){
            //Debug.Log("ALARM");
            if (rof_count > 0) {
                rof_count--;
            }
            if (isAIControled && rof_count == 0){
                findAndDestoryEnemy();
            }

        }else{
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log(targetPosition);
                //finalPosition = Vector3.Normalize(targetPosition - transform.position);
                Instantiate(projectile,targetPosition,Quaternion.identity);
                rof_count = rof;
                if(shotgun){
                    targetPosition.x--;
                    targetPosition.y--;

                    for(int i = 0; i < 7;i++){
                        Instantiate(projectile,targetPosition,Quaternion.identity);
                        targetPosition.x+=0.33f;
                        targetPosition.y+=0.33f;
                    }
                }
            }
        }
    }

    private Transform findAndDestoryEnemy(){
        GameObject player = manager.getAICha();
        Collider2D col = Physics2D.OverlapCircle(player.transform.position,5f);
        if(col != null){
            if(col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("Guard_Enemy")){
                targetPosition = col.gameObject.transform.position;
                
                Instantiate(projectile,targetPosition,Quaternion.identity);
                rof_count = rof;
                if(shotgun){
                    targetPosition.x--;
                    targetPosition.y--;
                    for(int i = 0; i < 7;i++){
                        Instantiate(projectile,targetPosition,Quaternion.identity);
                        targetPosition.x+=0.33f;
                        targetPosition.y+=0.33f;
                    }
                }

            }
        }
        return player.transform;
    }
}
*/