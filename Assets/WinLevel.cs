using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevel : MonoBehaviour
{
    public GameObject EndPanel;

    private bool hasWon;
    public bool winSignal;

    private void Start()
    {
        hasWon = false;
        winSignal = false;
        StartCoroutine(db());
    }

    IEnumerator db(){
        while (!hasWon)
        {
            //if(GameObject.FindGameObjectsWithTag("EnemyTower").Length <= 0)
            if(winSignal)
            {
                winSignal = false;
                hasWon = true;
                Time.timeScale = 0f;
                EndPanel.SetActive(true);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(!hasWon && GameObject.FindGameObjectsWithTag("EnemyTower").Length <= 0)
        //{
        //    hasWon = true;
        //    Time.timeScale = 0f;
        //    EndPanel.SetActive(true);
        //}
    }
}
