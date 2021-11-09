using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyDamagePlayer : MonoBehaviour
{
    public float damageDealt = 20;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Assert.IsNotNull(collision.gameObject.GetComponent<PlayerHealth>());
            collision.gameObject.GetComponent<PlayerHealth>().loseHp(damageDealt);
        }
    }
}
