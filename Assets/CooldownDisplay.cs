using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownDisplay : MonoBehaviour
{
    private GameObject playerController;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("CharacterManager");
    }

    // Update is called once per frame
    void Update()
    {
        player = playerController.GetComponent<PlayerController>().playerCharacter;


    }
}
