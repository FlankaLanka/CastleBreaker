using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimationChangeBoss : MonoBehaviour
{
    private Animator anim;
    private Animator weaponAnim;
    private Animator coverAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        weaponAnim = transform.Find("testweapons").GetComponent<Animator>();
        coverAnim = transform.Find("testcovers").GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            anim.SetTrigger("Bow");
            weaponAnim.SetTrigger("Bow");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetTrigger("Sword");
            weaponAnim.SetTrigger("Sword");
            coverAnim.SetTrigger("Sword");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            anim.SetTrigger("Scythe");
            weaponAnim.SetTrigger("Scythe");
            coverAnim.SetTrigger("Scythe");

        }
    }
}
