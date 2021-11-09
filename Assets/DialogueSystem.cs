using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Queue<KeyValuePair<string,string>> dialogueLines;

    private GameObject dialogueCanvas;
    private GameObject speakerImage;
    private GameObject speakerNameText;
    private GameObject speakerSpeakText;

    //public bool dialogueHappening = false;
    private bool alreadySetLine = false;


    private void Awake()
    {
        dialogueLines = new Queue<KeyValuePair<string, string>>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueCanvas = GameObject.Find("DialogueCanvas");
        speakerImage = dialogueCanvas.transform.Find("SpeakerImage").gameObject;
        speakerNameText = dialogueCanvas.transform.Find("TextPanel").Find("SpeakerNameText").gameObject;
        speakerSpeakText = dialogueCanvas.transform.Find("TextPanel").Find("SpeakerSpeakText").gameObject;
        dialogueCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueLines.Count > 0)
        {
            dialogueCanvas.SetActive(true);
            Time.timeScale = 0f;
            
            if(!alreadySetLine)
            {
                speakerNameText.GetComponent<Text>().text = dialogueLines.Peek().Key;
                speakerSpeakText.GetComponent<Text>().text = dialogueLines.Peek().Value;
                alreadySetLine = true;
            }
        }
    }

    public void NextSentence()
    {
        dialogueLines.Dequeue();
        if (dialogueLines.Count <= 0)
        {
            dialogueCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
        alreadySetLine = false;
    }

}
