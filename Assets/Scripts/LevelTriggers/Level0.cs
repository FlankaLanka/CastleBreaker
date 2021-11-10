using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Level0 : MonoBehaviour
{
    public GameObject magician; 
    public GameObject shooter; 
    public GameObject switchButton;
    public GameObject player_2_HP_bar;
    public List<GameObject> teams;
    public List<GameObject> flags;

    public GameObject Monsterfactory;
    // Start is called before the first frame update
    public GameObject win;
    //public TMP_Text textBar;
    private WinLevel w;

    private Queue<KeyValuePair<string, string>> dialogueLines;
    private GameObject dialogueManager;

    
    private SpawnEnemy mf;
    void Start()
    {
        dialogueManager = GameObject.Find("DialogueManager");
        dialogueLines = new Queue<KeyValuePair<string, string>>();

        switchButton.SetActive(false);
        player_2_HP_bar.SetActive(false);
        teams[4].SetActive(false);
        StartCoroutine(checkConditions());
        w = win.GetComponent<WinLevel>();
        mf = Monsterfactory.GetComponent<SpawnEnemy>();
        mf.setSpawn(false);
    }

    IEnumerator checkConditions(){
        float dis;
        float near = 25.0f;
        bool notHaveP2 = true;
        while (notHaveP2) {
            textUpdate();
            dis = (magician.transform.position - shooter.transform.position).sqrMagnitude;
            if(dis < near){
                switchButton.SetActive(true);
                player_2_HP_bar.SetActive(true);
                //Monsterfactory.SetActive(true);
                mf.setSpawn(true);
                //textBar.text = "Mission 3: Destory Tower in the North. Magician's Skills can quickly deal with a lot of enemies. ";
                Invoke("awakeTeam",5.0f);
                notHaveP2 = false;

                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "Nash! It's you! You don't know how long I've been searching for you."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Nash", "Quiet now! The androids will spot us."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Nash", "Although I'm slower than you. I have powerful AOE attacks. I'll take care of the android group ahead."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Nash", "You, go find the enemy Spawner. " +
                    "It is the large tower with their local network. Destroy that to cut off all androids in this area."));
                dialogueManager.GetComponent<DialogueSystem>().dialogueLines = dialogueLines;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }


    private int currentState = 0;
    private bool hasChanged = false;
    private bool line0Ready = true;
    private void textUpdate(){
        if (hasChanged || line0Ready) {
            switch (currentState)
            {
                case 1:
                    //textBar.text = "Mission 2: Find Magician Fox";
                    break;
                case 2:
                    //textBar.text = "Mission 2: Find Magician Fox. 6 ClockMK2 is really hard, find another way. ";
                    break;
                case 3:
                    //textBar.text = "Mission 2: Find Magician Fox. Pillboxs are dangerous. Try dashing through when they stop fire. ";
                    break;
                case 4:
                    //textBar.text = "Mission 3: Destory Tower in the North. Magician's Skills can quickly deal with a lot of enemies. ";
                    break;
                default:
                    break;
            }


            dialogueLines.Clear();
            if (currentState == 0)
            {
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "Looks like I'm in the warzone."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "As much as I want to take down these androids, I should probably find Nash first."));
                line0Ready = false;
            }
            else if (currentState == 1)
            {
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "I can sense Nash's presence. The magicican's aura, I feel it."));
            }
            else if (currentState == 2)
            {
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "It is difficult to approach the shooter androids in this tight space."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "Maybe I can find a way around."));
            }
            else if (currentState == 3)
            {
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "These androids seemed to have upgraded their technology."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "Burst Towers fire bullets rapidly. Watch out when they turn red."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "There must be a way I can get through them without sustaining too much damage."));
            }
            else if (currentState == 4)
            {
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Shelly", "Nash! It's you! You don't know how long I've been searching for you."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Nash", "Quiet now! The androids will spot us."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Nash", "Although I'm slower than you. I have powerful AOE attacks. I'll take care of the android group ahead."));
                dialogueLines.Enqueue(new KeyValuePair<string, string>("Nash", "You, go find the enemy Spawner. " +
                    "It is the large tower with their local network. Destroy that to cut off all androids in this area."));
            }

            dialogueManager.GetComponent<DialogueSystem>().dialogueLines = dialogueLines;
            hasChanged = false;

        }else{
            switch (currentState)
            {
                case 0:
                    if((flags[0].transform.position - shooter.transform.position).sqrMagnitude < 36.0f){
                        currentState = 1;
                        hasChanged = true;
                    }
                    break;
                case 1:
                    if((flags[0].transform.position - shooter.transform.position).sqrMagnitude < 36.0f){
                        currentState = 2;
                        hasChanged = true;
                    }
                    break;
                case 2:
                    if((flags[1].transform.position - shooter.transform.position).sqrMagnitude < 25.0f){
                        currentState = 3;
                        hasChanged = true;
                    }
                    break;
                case 3:
                    if((magician.transform.position - shooter.transform.position).sqrMagnitude < 25.0f){
                        currentState = 4;
                        hasChanged = true;
                    }
                    break;
                default:
                    break;
            }

        }
    }

    private void awakeTeam(){
        teams[4].SetActive(true);
    }
}
