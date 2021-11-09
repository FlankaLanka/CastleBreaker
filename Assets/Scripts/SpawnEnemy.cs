using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum General_Enemy_States {
    SLEEP,
    ALARM,
    ATTACK
}



public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    private int maxEnemies = 20;
    private float spawnTimer;
    public float alarmTimer;
    public float attackTimer; //set spawn speed based on different states
    public int guardEnemyMin;
    public int enemyTowerMin; //if towers or guard less than certain number then switch to more aggro state
    [SerializeField] private bool timerCanSpawn = true;
    private int canCheckCount = 100;


    private General_Enemy_States state = General_Enemy_States.SLEEP;

    // Start is called before the first frame update
    private void Start()
    {
        timerCanSpawn = true;
    }




    private bool isAwake = true;
    public void setSpawn(bool b) {
        isAwake = b;
    }
    // Update is called once per frame
    void Update()
    {
        if (canCheckCount == 0){
            canCheckCount = 100;
        }else{
            canCheckCount--;
        }

        if(isAwake){
            updateStates();
        }
    }



    private void updateStates(){
        switch (state)
        {
            case General_Enemy_States.SLEEP:
            {
                if (canCheckCount==100){
                    if ((GameObject.FindGameObjectsWithTag("Guard_Enemy").Length < guardEnemyMin)){
                        Debug.Log("SWITCH TO ALARM");
                        state = General_Enemy_States.ALARM;
                        spawnTimer = alarmTimer;
                    }
                    if((GameObject.FindGameObjectsWithTag("EnemyTower").Length < enemyTowerMin)){
                        Debug.Log("SWITCH TO ATTACK");
                        state = General_Enemy_States.ATTACK;
                    }
                }
                break;
            }
            case General_Enemy_States.ALARM:
            {
                if (canCheckCount==100){
                    if((GameObject.FindGameObjectsWithTag("EnemyTower").Length < enemyTowerMin)){
                        Debug.Log("SWITCH TO ATTACK");
                        state = General_Enemy_States.ATTACK;
                        spawnTimer = attackTimer;
                    }
                }
                if(timerCanSpawn){
                    timerCanSpawn = false;
                    spawnTimer = alarmTimer;
                    StartCoroutine(SpawnEnem());
                }
                break;
            }
            case General_Enemy_States.ATTACK:
            {
                if(timerCanSpawn){
                    timerCanSpawn = false;
                    spawnTimer = attackTimer;
                    StartCoroutine(SpawnEnem());
                }
                break;
            }
            default:
            break;
        }
    }


    IEnumerator SpawnEnem()
    {
        yield return new WaitForSeconds(spawnTimer);
        if ((GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)){
            Instantiate(enemyPrefab, transform.position - new Vector3(2f, 3.5f, 0f), Quaternion.identity);
        }
        timerCanSpawn = true;
    }
}
