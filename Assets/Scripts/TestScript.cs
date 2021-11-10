using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usefuls;

public class TestScript : MonoBehaviour
{
    private bool reached = false;
    public GameObject player;
    // Update is called once per frame

    private ChaAction chaAction;
    private ChaState chaState;

    private void Start() {
        chaAction = player.GetComponent<ChaAction>();
        chaState = player.GetComponent<ChaState>();
        chaState.isAIControled = true;
    }
    void Update()
    {
        if(reached){
            objectStop();
        }else{
            objectMoveForward();
        }
    }

    private void objectStop(){
        chaAction.stop();
        chaAction.switchControlToPlayer();
        Destroy(gameObject);
    }
    private void objectMoveForward(){
        Vector3 path = transform.position - player.transform.position;
        Vector2 d = path.normalized;
        chaAction.move(d.x,d.y);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.layer == Usefuls.UsefulConstant.PlayerLayer){
            Debug.Log("Reached");
            reached = true;
        }
    }
}
