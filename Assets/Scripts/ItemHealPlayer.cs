using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemHealPlayer : MonoBehaviour
{
    public float healAmt = 10;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Assert.IsNotNull(collision.gameObject.GetComponent<PlayerHealth>());
            collision.gameObject.GetComponent<PlayerHealth>().recoverHp(healAmt);
            Destroy(gameObject);
        }
    }
}
