using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


// All player's action should been handled here. 
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> characters; 

    [SerializeField]
    private Camera playerCamera;

    public GameObject playerCharacter;
    private int aiPlayer = 1;

    private ChaAction playerAction;
    private ChaSkillLauncher playerShooter;

    public Joystick moveStick;


    private GameObject cd1;
    private GameObject cd2;
    private GameObject cd3;

    // Start is called before the first frame update
    void Start()
    {
        if (characters.Count > 0){
            playerCharacter = characters[0];
            playerAction = playerCharacter.GetComponent<ChaAction>();
            playerShooter = playerCharacter.GetComponent<ChaSkillLauncher>();
            cameraFollow();
            //changePlayerCharacter(0);
        }
        else{
            Debug.Log("Cannot get valided character, editor may not set players. ");
        }

        cd1 = GameObject.Find("CooldownLayer1");
        cd2 = GameObject.Find("CooldownLayer2");
        cd3 = GameObject.Find("CooldownLayer3");

    }

    // Update is called once per frame
    void Update()
    {
        ShowCooldown(cd1, 0);
        ShowCooldown(cd2, 1);
        ShowCooldown(cd3, 2);

        playerAction.move(moveStick.Horizontal, moveStick.Vertical);

        // Move camera; 
        cameraFollow();
    }

    private void ShowCooldown(GameObject cdLayer, int skillNum)
    {
        float totalCD = playerAction.GetComponent<ChaState>().skills[skillNum].cooldown;
        Assert.IsNotNull(playerAction.GetComponent<ChaState>().skills[skillNum].skillPrefeb.GetComponent<Skill>());
        float remainingCD = GameObject.Find(playerAction.GetComponent<ChaState>().skills[skillNum].skillPrefeb.name + "(Clone)").
             GetComponent<Skill>().timer;
        cdLayer.GetComponent<Image>().fillAmount = 1 - remainingCD/totalCD;
    }

    private void cameraFollow() {
        Vector3 playPosition = playerCharacter.transform.position;
        playPosition.z = -10;
        playerCamera.transform.position = playPosition;
    }

    public GameObject getAICha(){
        return characters[aiPlayer];
    }


    //Below are functions for skill use on button press
    public void changePlayerCharacter()
    {
        if (Time.timeScale != 0f)
        {
            int characterID = aiPlayer;
            aiPlayer++;
            if (aiPlayer == 2)
                aiPlayer = 0;

            playerAction.stop();
            playerCharacter = characters[characterID];


            playerAction = playerCharacter.GetComponent<ChaAction>();
            playerShooter = playerCharacter.GetComponent<ChaSkillLauncher>();
        }
    }

    public void UseAttack1()
    {
        if(Time.timeScale != 0f)
            playerShooter.use_skill_1();
    }

    public void UseAttack2()
    {
        if (Time.timeScale != 0f)
            playerShooter.use_skill_2();
    }

    public void UseAttack3()
    {
        if (Time.timeScale != 0f)
            playerShooter.use_skill_3();
    }


}
