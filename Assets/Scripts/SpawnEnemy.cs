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
    public int maxEnemies{private get;set;}
    public float spawnTimer{private get;set;}
    public bool isAwake{private get;set;}


    private Queue<GameObject> myChilds;
    // public float alarmTimer;
    // public float attackTimer; //set spawn speed based on different states
    // public int guardEnemyMin;
    // public int enemyTowerMin; //if towers or guard less than certain number then switch to more aggro state
    // [SerializeField] private bool timerCanSpawn = true;
    // private int canCheckCount = 100;


    private General_Enemy_States state = General_Enemy_States.SLEEP;

    // Start is called before the first frame update
    private void Start()
    {
        isAwake = false;
        maxEnemies = 5;
        spawnTimer = 2.5f;
        myChilds = new Queue<GameObject>();
        StartCoroutine(SpawnEnem());
        // timerCanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAwake && myChilds.Count > 0){
            if(myChilds.Peek()!=null){
                myChilds.Enqueue(myChilds.Dequeue());
            }else{
                myChilds.Dequeue();
            }
        }
    }



/*
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

*/


    public void setSpawn(bool b) {
        isAwake = b;
    }

    public void setSpawnTimer(float t) {
        spawnTimer = t;
    }

    IEnumerator SpawnEnem()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTimer);
            if (isAwake && (myChilds.Count < maxEnemies)){
                myChilds.Enqueue(Instantiate(enemyPrefab, transform.position - new Vector3(2f, 3.5f, 0f), Quaternion.identity));
            }
        }
    }
}
