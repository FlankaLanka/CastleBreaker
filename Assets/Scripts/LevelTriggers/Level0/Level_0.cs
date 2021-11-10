using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelEvents;
public class Level_0 : LevelEventManager
{
    // Level players
    public GameObject shooterFox; 
    public GameObject magicianFox; 
    public GameObject theifFox;
    public GameObject swordFox;
    public GameObject Stones;
    public GameObject WinManager;


    // UI items
    // public GameObject switchButton;
    // public GameObject player_2_HP_bar;

    // Enemies teams
    // public List<GameObject> teams;

    // deprecate, use WayPoint insead
    // public List<GameObject> flags;

    // public GameObject Monsterfactory;
    // Start is called before the first frame update
    // public GameObject win;
    //public TMP_Text textBar;
    // private WinLevel w;


    
    // private SpawnEnemy mf;

    protected override void LevelStart(){
        Stones.SetActive(false);
        EnemiesTeams[0].SetActive(false);
        EnemiesTeams[2].SetActive(false);
        BlockPlayerBar(1);
        BlockSwitchButtom();
    }

    protected override void LevelUpdate(){    }


    protected override void SwitchAllCharacterToAI(){
        shooterFox.GetComponent<ChaState>().isAIControled = true; 
        magicianFox.GetComponent<ChaState>().isAIControled = true;
        theifFox.GetComponent<ChaState>().isAIControled = true;
        swordFox.GetComponent<ChaState>().isAIControled = true;
    }
    protected override void SwitchAllCharacterToPlayer(){
        shooterFox.GetComponent<ChaState>().isAIControled = false; 
        magicianFox.GetComponent<ChaState>().isAIControled = false;
        theifFox.GetComponent<ChaState>().isAIControled = false;
        swordFox.GetComponent<ChaState>().isAIControled = false;
    }


    protected override void RegisterAllEventTriggers(){

        // Trigger#0, At level beginning, Start CG#1, Block player input. 
        RegisterEventTrigger(
            delegate {
                return true;
                }, 
            delegate {
                BlockPlayerInput();
                StartCoroutine(MovePlayerToWayPoint(shooterFox, wayPoints[0]));
                },
            "In beginning, Start CG#1, Block player input and moving shooter to position");
        // Trigger#1, In CG#1, Watching for 1.0s, then start dialogue, after dialogue wa.
        RegisterEventTrigger(
            delegate {
                return playerEnteredWayPoint(wayPoints[0]);
                }, 
            delegate {
                float watchingTime = 1.0f;
                Invoke("dialogues_1", watchingTime);
                StartCoroutine(MovePlayerToWayPoint(shooterFox, wayPoints[1], watchingTime + 1f));
                StartCoroutine(SlowlyFadingScreen(true, watchingTime + 1.5f));
                },
            "when move to the position, Watching 1 sec and make dialouge #1, then move back and mask screen");
        // Trigger#2, In CG#1, Enter WayPoint#1.
        RegisterEventTrigger(
            delegate {
                return playerEnteredWayPoint(wayPoints[1]);
                }, 
            delegate {
                    EnemiesTeams[1].SetActive(false);
                    StartCoroutine(SlowlyFadingScreen(false));
                    StartCoroutine(MovePlayerToWayPoint(shooterFox, wayPoints[2]));
                    StartCoroutine(enableObject(EnemiesTeams[0],1.5f));
                },
            "When approched waypoint#1, unmask the screen, shooter moving to waypoint#2, and monster team#0 awaking in 1 sec");
        // Trigger#3, In CG#1, Enter WayPoint#2, Cut backpath.
        RegisterEventTrigger(
            delegate {
                return playerEnteredWayPoint(wayPoints[2]);
                }, 
            delegate {
                    Stones.SetActive(true);
                    Invoke("dialogues_2",2.0f);
                    Invoke("UnblockPlayerInput",2.2f);
                },
            "When approched waypoint#2, spwan Explosion and stone in wayPoint#3");
        //CG#1 Stopped. 

        // Trigger#4, On player enter wayPoint#3, Tell player should find other way.
        RegisterEventTrigger(
            delegate {
                return playerEnteredWayPoint(wayPoints[3]);
                }, 
            delegate {
                    dialogues_3();
                    Invoke("dialogues_4",1.5f);
                },
            "When player enter wayPoint#3, Tell player should find other way");
        // Trigger#5, On player enter wayPoint#4, Tell player should avoid Burst Towers.
        RegisterEventTrigger(
            delegate {
                return playerEnteredWayPoint(wayPoints[4]);
                }, 
            delegate {
                    Invoke("dialogues_5",1.5f);
                },
            "When player enter wayPoint#4, Tell player should avoid Burst Towers.");
        // Trigger#6, On player enter wayPoint#6, Raise dialogues between shooter and magicianFox.
        RegisterEventTrigger(
            delegate {
                return playerEnteredWayPoint(wayPoints[6]);
                }, 
            delegate {
                    Invoke("dialogues_6",0.0f);
                    UnblockPlayerBar(1);
                    UnblockSwitchButtom();
                    EnemiesTeams[2].SetActive(true);
                },
            "When player enter wayPoint#6, Raise dialogues between shooter and magicianFox");

        // Trigger#7, Make Player win the level.
        RegisterEventTrigger(
            delegate {
                    return GameObject.FindGameObjectsWithTag("EnemyTower").Length <= 0;
                }, 
            delegate {
                    WinManager.GetComponent<WinLevel>().winSignal = true;
                },
            "When player enter wayPoint#6, Raise dialogues between shooter and magicianFox");


    }

    // Dialogues. Unbeautiful method, but I did not find good idea for that. 
    private void dialogues_1(){
        PushDialogue("Shelly","Looks like I'm in the warzone.");
        PushDialogue("Shelly", "As much as I want to take down these androids, I should probably find Nash first.");
        RaiseDialogue();
    }
    private void dialogues_2(){
        PushDialogue("Shelly","A Trap! I can't move back.");
        PushDialogue("Shelly", "Those androids seems Unfriendly... I must deal with them first!");
        RaiseDialogue();
    }

    private void dialogues_3(){
        PushDialogue("Shelly","I can sense Nash's presence. The magicican's aura, I feel it.");
        RaiseDialogue();
    }

    private void dialogues_4(){
        PushDialogue("Shelly","It is difficult to approach the shooter androids in this tight space.");
        PushDialogue("Shelly", "Maybe I can find a way around.");
        RaiseDialogue();
    }
    private void dialogues_5(){
        PushDialogue("Shelly","These androids seemed to have upgraded their technology.");
        PushDialogue("Shelly", "Burst Towers fire bullets rapidly. Watch out when they turn red.");
        PushDialogue("Shelly", "There must be a way I can get through them without sustaining too much damage.");
        RaiseDialogue();
    }

    private void dialogues_6(){
        PushDialogue("Shelly","Nash! It's you! You don't know how long I've been searching for you.");
        PushDialogue("Nash", "Quiet now! The androids will spot us.");
        PushDialogue("Nash", "Although I'm slower than you. I have powerful AOE attacks. I'll take care of the android group ahead.");
        PushDialogue("Nash", "You, go find the enemy Spawner. " +
        "It is the large tower with their local network. Destroy that to cut off all androids in this area.");
        RaiseDialogue();
    }


}
