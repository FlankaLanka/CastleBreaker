using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelEvents;
public class Level_2 : LevelEventManager
{
    // Level players
    public GameObject shooterFox; 
    public GameObject magicianFox; 
    public GameObject theifFox; // Only need in CG
    public GameObject swordFox; // Only need in CG
    public List<GameObject> MonsterFactors;
    public List<GameObject> Towers;
    public GameObject Boss;
    private PlayerController playerController;


    protected override void LevelStart(){
        playerController = playerManager.GetComponent<PlayerController>();
        SetFactoryAttr(0,10,2.0f);
        SetFactoryAttr(1,5,3f);
        SetFactoryAttr(2,5,3f);
        SetFactoryAttr(3,10,2.0f);
        SetFactoryAttr(4,4,3f);
        SetFactoryAttr(5,4,3f);


        // Level Start......Wait until towers shutdown. 
    }

    private int destoriedFactories;
    protected override void LevelUpdate(){    
        // check number of towers
        destoriedFactories = 0;
        foreach(GameObject mf in MonsterFactors){
            if (mf == null){
                destoriedFactories++;
                //Debug.Log(destoriedFactories);
            }
        }
    }


    protected override void SwitchAllCharacterToAI(){
        swordFox.GetComponent<ChaState>().isAIControled = true;
        theifFox.GetComponent<ChaState>().isAIControled = true;
    }
    protected override void SwitchAllCharacterToPlayer(){
        swordFox.GetComponent<ChaState>().isAIControled = false;
        theifFox.GetComponent<ChaState>().isAIControled = false;
    }


    protected override void RegisterAllEventTriggers(){

        RegisterEventTrigger(
            delegate {
                    return true;
                }, 
            delegate {
                    theifFox.SetActive(false);
                    swordFox.SetActive(false);
                    BlockPlayerInput();
                    BlockPlayerBar(0);
                    BlockPlayerBar(1);
                    Invoke("dialogues_1", 1.0f);
                    Invoke("StartLevel", 3.0f);
                },
            "In beginning, Start CG#1, Block player input and moving shooter to position");

        // Trigger#0, 
        RegisterEventTrigger(
            delegate {
                    return destoriedFactories >= 6;
                }, 
            delegate {
                    Boss.SetActive(true);
                    dialogues_3();
                },
            "In beginning, Start CG#1, Block player input and moving shooter to position");

        RegisterEventTrigger(
            delegate {
                    return Boss && Boss.activeInHierarchy && (
                        (magicianFox.transform.position - Boss.transform.position).sqrMagnitude < 36.0f ||
                        (shooterFox.transform.position - Boss.transform.position).sqrMagnitude < 36.0f 
                        );
                }, 
            delegate {
                    dialogues_4();
                },
            "In beginning, Start CG#1, Block player input and moving shooter to position");


        RegisterEventTrigger(
            delegate {
                    return !(Boss);
                }, 
            delegate {
                    Invoke("dialogues_5", 1.0f);
                    Invoke("EndLevel", 1.5f);
                },
            "Win after player destory Power Generator");

    }

    private void StartLevel(){
        ShutdownAllTower();
        Invoke("dialogues_2", 2.0f);
        Invoke("StartLevel_2", 2.5f);
    }
    private void StartLevel_2(){
        theifFox.transform.position = wayPoints[5].transform.position;
        swordFox.transform.position = wayPoints[6].transform.position;

        UnblockPlayerInput();
        UnblockPlayerBar(0);
        UnblockPlayerBar(1);
        AwakeAllEnemies();
    }

    private void EndLevel(){
        BlockPlayerInput();
        BlockPlayerBar(0);
        BlockPlayerBar(1);
        StartCoroutine(SlowlyFadingScreen(true,0,6.0f)); 
        Invoke("EndLevel_2",4.0f);
    }
    private void EndLevel_2(){
        magicianFox.transform.position = wayPoints[2].transform.position;
        shooterFox.transform.position = wayPoints[3].transform.position;

        GameObject[] normal_em = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] guard_em = GameObject.FindGameObjectsWithTag("Guard_Enemy");
        foreach(GameObject em in normal_em){
            em.SetActive(false);
        }
        foreach(GameObject em in guard_em){
            em.SetActive(false);
        }

        StartCoroutine(SlowlyFadingScreen(false,0,4.0f));
        theifFox.SetActive(true);
        swordFox.SetActive(true);
        Invoke("EndLevel_3",2.0f);
    }

    private void EndLevel_3(){
        StartCoroutine(MovePlayerToWayPoint(theifFox, wayPoints[0]));
        StartCoroutine(MovePlayerToWayPoint(swordFox, wayPoints[1]));
        Invoke("dialogues_6", 4.5f);
        Invoke("ToWin",5.5f);
    }

    private void ShutdownAllTower(){
        foreach(GameObject tower in Towers){
            tower.GetComponent<EnemyDefendTowerMovement>().PowerOff();
        }
    }

    private void SetFactoryAttr(int index, int maxChildren, float spawnPeriod){
        MonsterFactors[index].GetComponent<SpawnEnemy>().maxEnemies = maxChildren;
        MonsterFactors[index].GetComponent<SpawnEnemy>().spawnTimer = spawnPeriod;
    }

    private void AwakeAllEnemies(){
        foreach(GameObject mf in MonsterFactors){
            mf.GetComponent<SpawnEnemy>().isAwake = true;
        }
        foreach(GameObject team in EnemiesTeams){
            team.SetActive(true);
        }
    }

    private void ToWin(){
        WinManager.GetComponent<WinLevel>().winSignal = true;
    }

    // Dialogues. Unbeautiful method, but I did not find good idea for that. 
    private void dialogues_1(){
        PushDialogue("Nash","Burst Towers are still working......We should hide here until Hex and Chem destory the Power Generator in the forest. ");
        RaiseDialogue();
    }

    private void dialogues_2(){
        PushDialogue("Shelly","Hi Nash, look there! All Towers are black out!");
        PushDialogue("Shelly","Its seems that we should destory ALL FACTORY to enter the castle.");
        PushDialogue("Nash","Ok, Let's go. We should hurry up because Androids will realize they are assaulted very soon!");
        PushDialogue("","ALARM RING!!!!!!!!!!!!!");
        RaiseDialogue();
    }

    private void dialogues_3(){
        PushDialogue("Shelly","Ok, I think this should be the last one. Let's go into the castle and End this invasion.");
        RaiseDialogue();
    }
    private void dialogues_4(){
        PushDialogue("Boss","FOX! YOU SHALL NEVER PASS!!!!");
        PushDialogue("Shelly && Nash","GET OUT OF MY WAY, INVADER!");
        RaiseDialogue();
    }

    private void dialogues_5(){
        PushDialogue("Boss","You...cannot resist the Great Android Mester!");
        PushDialogue("Shelly","Nash, Let's wait here....I believe Hex and Chem will coming soon.");
        PushDialogue("Nash","Sure.");
        RaiseDialogue();
    }
    private void dialogues_6(){
        PushDialogue("Hex && Chem","Hu....Hu.....");
        PushDialogue("Hex","Sorry we are late here. Is everythings OK? ");
        PushDialogue("Shelly","Ok, now we have all Fox here. Android Mester are inside the castle, who want to destory it with me?");
        PushDialogue("Nash","I will wait outside. My magic might crush entire castle if I use them indoor.....");
        PushDialogue("Chem","Shelly, Let me go with you. Hex, could you stay with Nash outside? I am afraid there are Androids remained outside. ");
        PushDialogue("Hex","Sure Thing.");

        RaiseDialogue();
    }

}

