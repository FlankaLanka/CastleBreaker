using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth; //must set this in editor
    public float currentHealth;

    public Slider HpBar;

    private void Start()
    {
        //float hitPoint = 
        currentHealth = maxHealth;
        HpBar.maxValue = maxHealth;
        HpBar.value = currentHealth;
    }

    public void loseHp(float amt)
    {
        if(amt >= currentHealth)
        {
            currentHealth -= amt;
            if(name == "Boss")
            {
                //play death animation then destroy
                //this is done in BossAI script
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            currentHealth -= amt;
            HpBar.value = currentHealth;
        }
    }

    public bool isHurt(){
        return currentHealth < maxHealth;
    }
}
