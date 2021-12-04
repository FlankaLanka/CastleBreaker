using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelEvents;
public class Level_3 : LevelEventManager
{
    // Level players
    public GameObject shooterFox; 
    public GameObject theifFox; // Only need in CG

    public GameObject Boss;
    private PlayerController playerController;


    protected override void LevelStart(){
        playerController = playerManager.GetComponent<PlayerController>();


        // Level Start......Wait until towers shutdown. 
        // BlockPlayerInput();
        // BlockPlayerBar(0);
        // BlockPlayerBar(1);
        // Invoke("dialogues_1", 1.0f);
        // Invoke("StartLevel", 3.0f);
    }

    protected override void LevelUpdate(){}


    protected override void SwitchAllCharacterToAI(){
        shooterFox.GetComponent<ChaState>().isAIControled = true;
        theifFox.GetComponent<ChaState>().isAIControled = true;
    }
    protected override void SwitchAllCharacterToPlayer(){
        shooterFox.GetComponent<ChaState>().isAIControled = false;
        theifFox.GetComponent<ChaState>().isAIControled = false;
    }


    protected override void RegisterAllEventTriggers(){

        RegisterEventTrigger(
            delegate {
                    return true;
                }, 
            delegate {
                dialogues_1();
                },
            "In beginning, Start CG#1, Block player input and moving shooter to position");


        // Trigger#0, 
        RegisterEventTrigger(
            delegate {
                    return !(Boss);
                }, 
            delegate {
                Invoke("ToWin",1.5f);
                dialogues_2();
                },
            "In beginning, Start CG#1, Block player input and moving shooter to position");

    }

    private void StartLevel(){
    }
    private void StartLevel_2(){
    }

    private void ToWin(){
        WinManager.GetComponent<WinLevel>().winSignal = true;
    }

    // Dialogues. Unbeautiful method, but I did not find good idea for that. 
    private void dialogues_1(){
        PushDialogue("Shelly","This is the End of the invasion.");
        PushDialogue("Shelly","Android Master should be eliminate.");
        PushDialogue("Android Master","You and your Fox make me feel ridiculous.");
        PushDialogue("Android Master","This Land required preservation. Fox's destroying are meaningless.....");
        PushDialogue("Chem", "Talk is cheap. Show me your HP bar.");
        RaiseDialogue();
    }

    private void dialogues_2(){
        PushDialogue("Android Master","No! I...cannot be killed like this....");
        PushDialogue("Shelly","It's over, Android.");
        PushDialogue("Shelly","No Android runs forever......");
        PushDialogue("Android Master","System.....shutdown.");
        PushDialogue("Android Master",".........");
        PushDialogue("Android Master",".........");
        PushDialogue("Android Master",".........");
        PushDialogue("Chem", "Let's back. Hex and Nash are wait for us. ");
        PushDialogue("Shelly", "Fine. We have so much to rebuild, don't waste time. ");
    }


}

