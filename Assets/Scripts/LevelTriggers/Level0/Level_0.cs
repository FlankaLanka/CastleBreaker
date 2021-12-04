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

    public GameObject TargetMonsterfactory;
    
    public GameObject PowerGenetor;
    public List<GameObject> Towers;
    private PlayerController playerController;
    protected override void LevelStart(){
        playerController = playerManager.GetComponent<PlayerController>();
        Stones.SetActive(false);
        EnemiesTeams[0].SetActive(false);
        EnemiesTeams[2].SetActive(false);
        BlockPlayerBar(1);
        BlockSwitchButtom();

        EnemiesTeams[6].SetActive(false);
        EnemiesTeams[7].SetActive(false);
        EnemiesTeams[8].SetActive(false);
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
                BlockPlayerBar(0);
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
                    StartCoroutine(UnblockPlayerBarAfter(0,2.2f));
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

        // Trigger#7, On player destory the Tower, It found telephone and Raise dialogues.
        RegisterEventTrigger(
            delegate {
                    return TargetMonsterfactory==null;
                }, 
            delegate {
                    dialogues_7();
                },
            "When player destory the Tower, It found telephone and Raise dialogues");
        

        // Trigger#8, On player enter the telegraph, Block PlayerInput, then Raise dialogues, CG#2 beginning.
        // CG#2 beginning,  
        RegisterEventTrigger(
            delegate {
                    return playerEnteredWayPoint(wayPoints[5]);
                }, 
            delegate {
                    
                    // Block Input
                    // Instantiate()
                    Invoke("BlockPlayerInput", 0.125f);
                    BlockPlayerBar(0);
                    BlockPlayerBar(1);
                    shooterFox.GetComponent<ChaAction>().stop();
                    // Raise Dialogues. 
                    Invoke("dialogues_8",0.125f);


                    // I just use Time control everything here....
                    StartCoroutine(SlowlyFadingScreen(true, 0.5f, 3.0f));
                    
                    // Recycle Units in last Sence. 
                    Invoke("SwitchFoxGroup", 2.0f);

                    // CG#2 first part, theif move to wayPoint#7(basement door) after dialogues_9. 
                    // Move Camera. 
                    Invoke("MoveCameraToTheifBasement", 2.0f);
                    StartCoroutine(SlowlyFadingScreen(false, 2.0f, 3.0f));
                    Invoke("dialogues_9", 5.0f);
                    
                    //UnblockPlayerInput();
                    //UnblockPlayerBar(0);
                    //UnblockPlayerBar(1);
                    StartCoroutine(MovePlayerToWayPoint(theifFox, wayPoints[7], 5.1f));
                    StartCoroutine(SlowlyFadingScreen(true, 5.0f, 3.0f));
                },
            "When player enter the telegraph(wayPoints#5), Raise dialogues, Then Block PlayerInput, CG#2 First Part beginning.");
        
        // Trigger#9, CG#2 part#2. On theifFox enter the door, fading screen to black, then move Theif into the Bar.
        RegisterEventTrigger(
            delegate {
                    return playerEnteredWayPoint(wayPoints[7]);
                }, 
            delegate {
                    Invoke("SwitchFromBasementToBar", 1.0f);
                    // I just use Time control everything here....
                },
            "When theifFox enter the door, fading screen to black, then move Theif into the Bar.");

        // Trigger#10, CG#2 part#3. On theifFox near the SowrdFox, throw the dialogues, after dialogues, fading screen and move both fox outside the bar(waypoint 10,11). Finish the CG#2
        RegisterEventTrigger(
            delegate {
                    return playerEnteredWayPoint(wayPoints[9]);
                }, 
            delegate {
                    Invoke("dialogues_10", 1.0f);
                    StartCoroutine(SlowlyFadingScreen(true, 2.0f, 3.0f));
                    Invoke("EndingCG2", 5.0f);
                    StartCoroutine(SlowlyFadingScreen(false, 6.0f, 6.0f));
                    Invoke("dialogues_11", 7.0f);
                    Invoke("UnblockPlayerInput", 7.5f);
                    Invoke("UnblockSwitchButtom", 7.5f);
                    StartCoroutine(UnblockPlayerBarAfter(0,7.5f));
                    StartCoroutine(UnblockPlayerBarAfter(1,7.5f));
                },
            "When theifFox near the SowrdFox, throw the dialogues, after dialogues, fading screen and move both fox outside the bar(waypoint 10,11)");
            
        // Trigger#11, On foxes go near by the tower, hint player should destory the powergen first. 
        RegisterEventTrigger(
            delegate {
                    return playerEnteredWayPoint(wayPoints[12]);
                }, 
            delegate {
                    if(PowerGenetor != null){
                        dialogues_12();
                    }else{
                        dialogues_13();
                    }
                },
            "When foxes go near by the tower, hint player should destory the powergen first.");

        // Trigger#12, On power Genetor was destoried, raise dialogues.
        RegisterEventTrigger(
            delegate {
                    return PowerGenetor==null;
                }, 
            delegate {
                    foreach(GameObject df in Towers){
                        df.GetComponent<EnemyDefendTowerMovement>().PowerOff();
                    }
                    dialogues_14();
                },
            "When power Genetor was destoried, raise dialogues. ");

        // Trigger#13, On foxes go near by the BOSS, Raise dialogues_15.
        RegisterEventTrigger(
            delegate {
                    return playerEnteredWayPoint(wayPoints[13]);
                }, 
            delegate {
                        dialogues_15();
                },
            "When foxes go near by the BOSS, Raise dialogues_15.");

        // Trigger#14, Make Player win the level.
        RegisterEventTrigger(
            delegate {
                    return EnemiesTeams[8] == null;
                }, 
            delegate {
                    dialogues_16();
                    Invoke("ToWin",1.0f);
                },
            "When player enter wayPoint#6, Raise dialogues between shooter and magicianFox");

    }

    private void SwitchFoxGroup(){
        playerController.setCharacter(theifFox, 0);
        playerController.setCharacter(swordFox, 1);
        shooterFox.GetComponent<PlayerHealth>().recoverHp(500.0f);
        magicianFox.GetComponent<PlayerHealth>().recoverHp(500.0f);
        theifFox.GetComponent<PlayerHealth>().recoverHp(500.0f);
        swordFox.GetComponent<PlayerHealth>().recoverHp(500.0f);
        //shooterFox.SetActive(false);
        //magicianFox.SetActive(false);
        EnemiesTeams[0].SetActive(false);
        EnemiesTeams[1].SetActive(false);
        EnemiesTeams[2].SetActive(false);
        EnemiesTeams[3].SetActive(false);
        EnemiesTeams[4].SetActive(false);
        EnemiesTeams[5].SetActive(false);
    }
    private void MoveCameraToTheifBasement(){
        playerController.LockCameraToPlayer(false);
        playerController.MoveCameraTo(wayPoints[7].transform.position);
    }

    private void SwitchFromBasementToBar(){
        theifFox.GetComponent<ChaAction>().stop();
        theifFox.transform.position = wayPoints[8].transform.position;
        playerController.MoveCameraTo(theifFox.transform.position);
        playerController.LockCameraToPlayer(true);
        StartCoroutine(SlowlyFadingScreen(false, 0.5f, 4.0f));
        StartCoroutine(MovePlayerToWayPoint(theifFox, wayPoints[9],1.5f));
    }

    private void EndingCG2(){
        theifFox.transform.position = wayPoints[10].transform.position;
        swordFox.transform.position = wayPoints[11].transform.position;
        EnemiesTeams[6].SetActive(true);
        EnemiesTeams[7].SetActive(true);
        EnemiesTeams[8].SetActive(true);
    }

    private void ToWin(){
        WinManager.GetComponent<WinLevel>().winSignal = true;
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

    private void dialogues_7(){
        PushDialogue("Shelly","Look What I found! A Telegraphy!!");
        PushDialogue("Shelly", "I think I can send some note to my friend Chem, and it will alert all foxes. ");
        PushDialogue("Shelly", "Let me see see Chem's number.....That it, start sending text.....");
        RaiseDialogue();
    }
    private void dialogues_8(){
        PushDialogue("Shelly", "Let me see see Chem's number.....That it, start sending text.....");
        RaiseDialogue();
    }
    private void dialogues_9(){
        PushDialogue("Chem", "Whoes message...Shelly?");
        PushDialogue("Shelly's message", "This is Shelly.  Androids' Invasion Detected, raise RED ALERT!");
        PushDialogue("Chem", "Androids' Invasion? That's funny.....they living far away from us....");
        PushDialogue("Chem", "But Shelly is a serious Fox......"+"Better to find Hex and discuss about it first.");
        RaiseDialogue();
    }
    private void dialogues_10(){
        PushDialogue("Chem", "Hello Hex, I got a bad news.");
        PushDialogue("Chem", "Shelly told me that Androids are invading our village.");
        PushDialogue("Hex", "RE-----ALLY? Maybe you need some Fox wine to wake up");
        PushDialogue("Chem", "........");
        PushDialogue("Hex", "........");
        PushDialogue("Background Noise", "BOOM!!!!!!");
        PushDialogue("Chem", "What happen outside?");
        PushDialogue("Hex", "Let's check it!");
        RaiseDialogue();
    }
    private void dialogues_11(){
        PushDialogue("Chem", "Seems that Shelly was right. Androids is now attacking our village. ");
        PushDialogue("Hex", "CRASH THOSE SCRAPS!!!!");
        PushDialogue("Chem", "........");
        PushDialogue("Chem", "You drunk too much.....");
        PushDialogue("Chem", "Those Androids are too much to resist. We should runaway to the forest on The North. ");
        PushDialogue("Chem", "Let's go, Hex. We should Survive first.");
        RaiseDialogue();
    }
    private void dialogues_12(){
        PushDialogue("Chem", "Burst Towers! Watch Out!");
        PushDialogue("Hex", "Damn those Androids.....We need to find some way to make them Shutdown!");
        PushDialogue("Chem", "I have a idea.....I saw a power genetor on the West, may be we can try to destory it first.");
        RaiseDialogue();
    }


    private void dialogues_13(){
        PushDialogue("Chem", "Burst Towers! Watch Out!");
        PushDialogue("Hex", "Calm Down, Chem. Those towers seems unfunctional now.......We could just walk through them. ");        
        RaiseDialogue();
    }
    private void dialogues_14(){
        PushDialogue("Hex", "Ok, the Power Generator has been destoried. All Androids defense should be shutdown. ");        
        RaiseDialogue();
    }

    private void dialogues_15(){
        PushDialogue("BOSS", "You Shall Not Pass!");
        PushDialogue("Hex", "A Androids boss! We should defeat it! ");        
        PushDialogue("Chem", "Let's go. ");        
        RaiseDialogue();
    }
    private void dialogues_16(){
        PushDialogue("Hex", "The Boss was defeated. Let's go to the forest! ");
        PushDialogue("Chem", "Nash and Shelly are wait for us in the forest. We should make plan to resist all anroids.");        
        RaiseDialogue();
    }
}
