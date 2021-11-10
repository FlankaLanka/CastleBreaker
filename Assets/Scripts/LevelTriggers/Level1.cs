using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1 : MonoBehaviour
{
    public GameObject thief; 
    public GameObject swordsfox; 
    public List<GameObject> defendTowers;
    public GameObject powerGen; 
    public TMP_Text textBar;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (powerGen==null){
            foreach(GameObject df in defendTowers){
                df.GetComponent<EnemyDefendTowerMovement>().PowerOff();
            }
        }
    }
}
