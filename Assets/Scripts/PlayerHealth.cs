using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHp;
    private float maxHp;
    //[SerializeField] 
    public bool canTakeDmg;
    private float iFrameTime;
    private bool isDead;


    public GameObject LosePanel;
    public GameObject HpBar;
    private float maxHpUIWidth;
    private float currentHpUIWidth;


    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        maxHpUIWidth = HpBar.GetComponent<RectTransform>().sizeDelta.x;
        currentHpUIWidth = HpBar.GetComponent<RectTransform>().sizeDelta.x;
        maxHp = 100f;
        currentHp = 100f;
        canTakeDmg = true;
        iFrameTime = 1f; //also knockback frame
        isDead = false;
    }

    private void Update()
    {
        if(currentHp <= 0)
        {
            if(!isDead)
            {
                isDead = true;
                Time.timeScale = 0f;
                LosePanel.SetActive(true);
            }
        }
    }

    public void loseHp(float amt)
    {
        if(canTakeDmg)
        {
            canTakeDmg = false;

            currentHp -= amt;
            float barShrinkAmt = maxHpUIWidth * (amt / maxHp);
            currentHpUIWidth -= barShrinkAmt;
            HpBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHpUIWidth);

            gameObject.GetComponent<ChaAction>().enabled = false;
            anim.SetTrigger("Hit");

            StartCoroutine(iFrame());
        }
    }

    public void recoverHp(float amt)
    {
        if(currentHp < maxHp)
        {
            if(currentHp + amt > maxHp)
            {
                amt = maxHp - currentHp;
            }
            currentHp += amt;
            float barGrowAmt = maxHpUIWidth * (amt / maxHp);
            currentHpUIWidth += barGrowAmt;
            HpBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHpUIWidth);
        }
    }

    IEnumerator iFrame() //for knockback, implement later
    {
        yield return new WaitForSeconds(iFrameTime);
        gameObject.GetComponent<ChaAction>().enabled = true;
        canTakeDmg = true;
    }
}
