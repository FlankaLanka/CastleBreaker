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

    private int currentPlayer;
    public GameObject playerCharacter;
    private int aiPlayer = 1;

    private ChaAction playerAction;
    private ChaSkillLauncher playerShooter;
    private ChaState playerState;

    public Joystick moveStick;

    public bool CGMovingCamera {get; private set;}
    private GameObject cd1;
    private GameObject cd2;
    private GameObject cd3;
    private Image buttonText_1;
    private Image buttonText_2;
    private Image buttonText_3;

    // Start is called before the first frame update
    void Start()
    {
        if (characters.Count > 0){
            currentPlayer = 0;
            playerCharacter = characters[0];
            playerAction = playerCharacter.GetComponent<ChaAction>();
            playerShooter = playerCharacter.GetComponent<ChaSkillLauncher>();
            playerState = playerCharacter.GetComponent<ChaState>();
            cameraFollow();
            //changePlayerCharacter(0);
        }
        else{
            Debug.Log("Cannot get valided character, editor may not set players. ");
        }
        CGMovingCamera = false;

        cd1 = GameObject.Find("CooldownLayer1");
        cd2 = GameObject.Find("CooldownLayer2");
        cd3 = GameObject.Find("CooldownLayer3");
        buttonText_1 = cd1.transform.parent.Find("AbilityImage").GetComponent<Image>();
        buttonText_2 = cd2.transform.parent.Find("AbilityImage").GetComponent<Image>();
        buttonText_3 = cd3.transform.parent.Find("AbilityImage").GetComponent<Image>();

        //buttonText_1.text = playerState.skills[0].skillName;
        //buttonText_2.text = playerState.skills[1].skillName;
        //buttonText_3.text = playerState.skills[2].skillName;

        buttonText_1.sprite = playerState.skills[0].skillImage;
        buttonText_2.sprite = playerState.skills[1].skillImage;
        buttonText_3.sprite = playerState.skills[2].skillImage;
    }

    // Update is called once per frame
    void Update()
    {
        ShowCooldown(cd1, 0);
        ShowCooldown(cd2, 1);
        ShowCooldown(cd3, 2);

        if(!playerState.isAIControled){
            playerAction.move(moveStick.Horizontal, moveStick.Vertical);
        }
        if(!CGMovingCamera){
            // Move camera; 
            cameraFollow();
        }
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

    public void setCharacter(GameObject newPlayer, int i){
        if(i < 0){
            Debug.Log("Invalid index!!");
            return;
        }

        if(i == currentPlayer){
            characters[i] = newPlayer;
            playerCharacter = characters[i];

            playerAction = playerCharacter.GetComponent<ChaAction>();
            playerShooter = playerCharacter.GetComponent<ChaSkillLauncher>();
            playerState = playerCharacter.GetComponent<ChaState>();

            //buttonText_1.text = playerState.skills[0].skillName;
            //buttonText_2.text = playerState.skills[1].skillName;
            //buttonText_3.text = playerState.skills[2].skillName;

            buttonText_1.sprite = playerState.skills[0].skillImage;
            buttonText_2.sprite = playerState.skills[1].skillImage;
            buttonText_3.sprite = playerState.skills[2].skillImage;


        }
        else{
            if(i < characters.Count){
                characters[i] = newPlayer;
            }else{
                Debug.Log("Invalid index!!");
                return;
            }
        }
    }

    public void LockCameraToPlayer(bool toLock){
        CGMovingCamera = !toLock;
    }

    public void MoveCameraTo(Vector3 targetPosition) {
        if(CGMovingCamera){
            Vector3 playPosition = new Vector3(targetPosition.x, targetPosition.y, -10);
            playerCamera.transform.position = playPosition;
        }
    }


    //Below are functions for skill use on button press
    public void changePlayerCharacter()
    {
        if (Time.timeScale != 0f)
        {
            int characterID = aiPlayer;
            aiPlayer++;
            //if (aiPlayer == 2)
            if(aiPlayer == characters.Count)
                aiPlayer = 0;

            playerAction.stop();
            currentPlayer = characterID;
            playerCharacter = characters[characterID];


            playerAction = playerCharacter.GetComponent<ChaAction>();
            playerShooter = playerCharacter.GetComponent<ChaSkillLauncher>();
            playerState = playerCharacter.GetComponent<ChaState>();

            //buttonText_1.text = playerState.skills[0].skillName;
            //buttonText_2.text = playerState.skills[1].skillName;
            //buttonText_3.text = playerState.skills[2].skillName;

            buttonText_1.sprite = playerState.skills[0].skillImage;
            buttonText_2.sprite = playerState.skills[1].skillImage;
            buttonText_3.sprite = playerState.skills[2].skillImage;

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
