using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClockMovement : MonoBehaviour
{
    private Transform closestPlayer;
    private GameObject[] playersList;
    public float speed = 1.5f;
    private Rigidbody2D rb;
    private float findTarget = 1.5f; //every 1.5 secs finds the new closest target
    //private PlayerController manager;
    //private int seek_time = 300;

    // Start is called before the first frame update
    void Start()
    {
        //manager = GameObject.Find("CharacterManager").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        playersList = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        player = manager.playerCharacter.transform;
        if(Vector2.Distance(player.position,gameObject.transform.position) > 1 && Vector2.Distance(player.position,gameObject.transform.position) < 5)
        {
            seek_time = 300;
            MoveToPlayer();
        }
        else
        {
            if (seek_time > 0){
                seek_time--;
                MoveToPlayer();
            }else{
                StopMovement();
            }
        }*/

        if(findTarget >= 1.5f)
        {
            findTarget = 0f;
            closestPlayer = FindClosestPlayer();
        }
        else
        {
            findTarget += Time.deltaTime;
        }

        MoveToPlayer();
    }


    private Transform FindClosestPlayer()
    {
        GameObject closest = playersList[0];
        Vector2 currentPosition = gameObject.transform.position;
        float minDistance = 9999999f;
        foreach(GameObject player in playersList)
        {
            Vector2 playerDist = player.transform.position;
            float distance = (playerDist - currentPosition).sqrMagnitude;

            if(distance < minDistance && player.layer == Usefuls.UsefulConstant.PlayerLayer)
            {
                minDistance = distance;
                closest = player;
            }
        }

        return closest.transform;
    }


    private void MoveToPlayer()
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, closestPlayer.position, Time.deltaTime * speed);
    }
    private void StopMovement()
    {
        //Debug.Log("STOP!!");
        rb.velocity = new Vector2(0, 0);
    }



    private void AttackPlayer()
    {
        //implement here
    }
}
