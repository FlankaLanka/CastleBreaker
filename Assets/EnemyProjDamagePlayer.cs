using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyProjDamagePlayer : MonoBehaviour
{
    public float damageDealt = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Assert.IsNotNull(collision.gameObject.GetComponent<PlayerHealth>());
            collision.gameObject.GetComponent<PlayerHealth>().loseHp(damageDealt);
        }
    }
}
