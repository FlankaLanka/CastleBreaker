using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelEvents;
public class Level_1 : LevelEventManager
{
    // Level players
    public GameObject swordFox; 
    public GameObject theifFox;    
    public GameObject PowerGenetor;
    public GameObject MonsterFactor_1;
    public GameObject MonsterFactor_2;

    public GameObject CampFire;

    private PlayerController playerController;


    protected override void LevelStart(){
        playerController = playerManager.GetComponent<PlayerController>();


        // At level beginning, Start CG#1, Block player input, And set camera position. 
    }

    protected override void LevelUpdate(){    }


    protected override void SwitchAllCharacterToAI(){
        swordFox.GetComponent<ChaState>().isAIControled = true;
        theifFox.GetComponent<ChaState>().isAIControled = true;
    }
    protected override void SwitchAllCharacterToPlayer(){
        swordFox.GetComponent<ChaState>().isAIControled = false;
        theifFox.GetComponent<ChaState>().isAIControled = false;
    }


    protected override void RegisterAllEventTriggers(){
        // Trigger#0, 
        RegisterEventTrigger(
            delegate {
                return true;
                }, 
            delegate {
                    BlockPlayerInput();
                    BlockPlayerBar(0);
                    BlockPlayerBar(1);
                    playerController.LockCameraToPlayer(false);
                    playerController.MoveCameraTo(wayPoints[0].transform.position);
                    dialogues_1();
                    Invoke("StartLevel", 1.0f);

                    MonsterFactor_1.GetComponent<SpawnEnemy>().isAwake = true;
                    MonsterFactor_2.GetComponent<SpawnEnemy>().isAwake = true;
                },
            "In beginning, Awake Factories");


        RegisterEventTrigger(
            delegate {
                    return PowerGenetor == null;
                }, 
            delegate {
                    Invoke("ToWin", 1.0f);
                },
            "Win after player destory Power Generator");

    }

    private void StartLevel(){
        playerController.MoveCameraTo(theifFox.transform.position);
        playerController.LockCameraToPlayer(true);
        StartCoroutine(SlowlyFadingScreen(true, 0, 10.0f));
        Invoke("StartLevel_2", 3.0f);
    }
    private void StartLevel_2(){
        UnblockPlayerInput();
        UnblockPlayerBar(0);
        UnblockPlayerBar(1);
        StartCoroutine(SlowlyFadingScreen(false, 0, 2.0f));
        CampFire.SetActive(false);
    }
    private void ToWin(){
        WinManager.GetComponent<WinLevel>().winSignal = true;
    }

    // Dialogues. Unbeautiful method, but I did not find good idea for that. 
    private void dialogues_1(){
        PushDialogue("Shelly","Hello everyone. Now we have to fight together.");
        PushDialogue("Shelly", "Let's discuss about how to resist those Invaders.");
        PushDialogue("Chem", "I scoped Androids' castle this morning.");
        PushDialogue("Chem", "It's well defensed, protected by tons of towers. We cannot strike that base directly.");
        PushDialogue("Nash", "Emmmm.....That's weird. Brust Tower need a lot of power to support. Did you see any Power Generator nearby?");
        PushDialogue("Chem", "No, I didn't found anything like Power Generator. Those scraps cleaned entire area and only towers and androids around the castle.");
        PushDialogue("Nash", "I guess the castle need power outside that area....maybe we should find it first......");
        PushDialogue("Hex", "I think I known where those scraps deploied their Power Generator.....");
        PushDialogue("Hex", "A week ago I found a weird building in the forest......I believe that was some fox's performance art in that time....");
        PushDialogue("Other Foxs", ".........");
        PushDialogue("Hex", "Well, now I realize that is a Power Generator.");
        PushDialogue("Nash", "Well....we can destory it first, and I believe that will shutdown all towers nearby the castle. ");
        PushDialogue("Nash", "Hex, could destory that Power Generator with Chem? "); 
        PushDialogue("Nash", "Shelly and I will go to wait around the castle and try to elimite all defenders");
        PushDialogue("Hex && Chem", "Sure. Let's do it tomorrow morning");

        RaiseDialogue();
    }

}

