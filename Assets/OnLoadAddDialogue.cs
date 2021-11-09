using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoadAddDialogue : MonoBehaviour
{
    public List<string> contentNames;
    [TextArea(4, 10)]
    public List<string> contentSays;

    
    private Queue<KeyValuePair<string,string>> enterSceneDialogue;
    private GameObject dialogueManager;

    void Start()
    {
        dialogueManager = GameObject.Find("DialogueManager");

        enterSceneDialogue = new Queue<KeyValuePair<string, string>>();
        int iter = 0;
        foreach(string name in contentNames)
        {
            enterSceneDialogue.Enqueue(new KeyValuePair<string, string>(name, contentSays[iter]));
            iter++;
        }

        dialogueManager.GetComponent<DialogueSystem>().dialogueLines = enterSceneDialogue;
    }
}
