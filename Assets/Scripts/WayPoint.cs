using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public int id;

    public bool areaTrigger = false;
    private bool reached = false;


    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,0.5f);

    #if UNITY_EDITOR
        float scale = UnityEditor.HandleUtility.GetHandleSize(transform.position);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.fontSize = (int)(60/scale);
        UnityEditor.Handles.Label(transform.position, ""+id,style);
    #endif

    }
    private void OnTriggerEnter2D(Collider2D c) {
        if(areaTrigger && !reached){
            if(c.gameObject.layer == Usefuls.UsefulConstant.PlayerLayer || c.gameObject.layer == Usefuls.UsefulConstant.Hidden){
                reached = true;
            }
        }
    }

    public bool playerReached(){
        return reached;
    }

    public void resetReachedState(){
        reached = false;
    }

}
