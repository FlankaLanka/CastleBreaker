using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossRadius : MonoBehaviour
{
    public int players_in_range = 0;
    public GameObject[] players;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            players_in_range++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            players_in_range--;
        }
    }
}
