using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using LevelEvents;


public abstract class LevelEventManager : MonoBehaviour {
        public static LevelEventManager Instance { get; private set; }

        public GameObject playerManager;
        public GameObject WinManager;


        // Useful public settings. Should be set well. 
        // WayPoints, should be add all. 
        public List<GameObject> WayPointObjs;

        // Some UI elements. 
        public GameObject PlayerInputCanvans;
        public GameObject SwitchButton;
        public List<GameObject> PlayerHPBars;


        // Enemies teams can also as triggeer. 
        public List<GameObject> EnemiesTeams;

        [SerializeField]
        private Camera playerCamera;
        private SpriteRenderer cameraMask = null;



        protected Dictionary<int, WayPoint> wayPoints;
        protected Queue<LevelEventTrigger> activedEventTriggers;
        protected Dictionary<int, LevelEventTrigger> allEventTriggers;

        protected Queue<KeyValuePair<string, string>> dialogueLines;
        protected DialogueSystem dialogueManager;

        

        private void Awake() {
            wayPoints = new Dictionary<int, WayPoint>();
            activedEventTriggers = new Queue<LevelEventTrigger>();
            allEventTriggers = new Dictionary<int, LevelEventTrigger>();
            dialogueLines = new Queue<KeyValuePair<string, string>>();
        }

        void Start()
        {
            dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueSystem>();
            
            foreach(GameObject g_obj in WayPointObjs){
                WayPoint wp = g_obj.GetComponent<WayPoint>();
                if(wayPoints.ContainsKey(wp.id)){
                    Debug.Log("WayPoint ID Duplicate!!!!");
                    Assert.IsTrue(false);
                }
                wayPoints.Add(wp.id,wp);
            }
            StartCoroutine(pollingEventTriggers());

            LevelStart();
            RegisterAllEventTriggers();
        }
        

        void Update()
        {
            LevelUpdate();
        }

        IEnumerator pollingEventTriggers(){
            while (true) {
                // poll all events;
                int limit = activedEventTriggers.Count;
                for(int i = 0; i < limit; i++){
                    LevelEventTrigger et = activedEventTriggers.Dequeue();
                    if(et.checkCondition()){
                        et.do_Consequence();
                    }
                    if(et.loop){
                        activedEventTriggers.Enqueue(et);
                    }else{
                        if(!et.hasTriggered){
                            activedEventTriggers.Enqueue(et);
                        }
                    }
                }
                yield return new WaitForSecondsRealtime(0.125f);
            }
        }

        protected abstract void RegisterAllEventTriggers();
        protected abstract void LevelStart();
        protected abstract void LevelUpdate();

        protected abstract void SwitchAllCharacterToAI();
        protected abstract void SwitchAllCharacterToPlayer();

        protected LevelEventTrigger RegisterEventTrigger(Func<bool> Condition, Action Consequence, string Description, bool loop = false){
            LevelEventTrigger et = new LevelEventTrigger(Condition,Consequence,Description,loop);
            activedEventTriggers.Enqueue(et);
            allEventTriggers.Add(et.id, et);
            return et;
        }






        // Buttoms, UI, Etcs; 
        // Open/Close
        protected void BlockPlayerInput(){
            SwitchAllCharacterToAI();
            PlayerInputCanvans.SetActive(false);
        }
        protected void UnblockPlayerInput(){
            SwitchAllCharacterToPlayer();
            PlayerInputCanvans.SetActive(true);
        }
        protected void BlockSwitchButtom(){
            SwitchButton.SetActive(false);
        }
        protected void UnblockSwitchButtom(){
            SwitchButton.SetActive(true);
        }

        protected void BlockPlayerBar(int index){
            PlayerHPBars[index].SetActive(false);
        }
        protected void UnblockPlayerBar(int index){
            PlayerHPBars[index].SetActive(true);
        }
        protected IEnumerator BlockPlayerBarAfter(int index, float waitSeconds){
            yield return new WaitForSeconds(waitSeconds);
            PlayerHPBars[index].SetActive(false);
        }
        protected IEnumerator UnblockPlayerBarAfter(int index, float waitSeconds){
            yield return new WaitForSeconds(waitSeconds);
            PlayerHPBars[index].SetActive(true);
        }



        // check If any player reached WayPoint
        protected bool playerEnteredWayPoint(WayPoint wp){
            return wp.playerReached();
        }

        // Move some Player to some point. 
        // Or wait some sec and move to that point. 
        protected IEnumerator MovePlayerToWayPoint(GameObject player, WayPoint wp, float waitUntilSecond=0.0f){
            ChaAction playerAction = player.GetComponent<ChaAction>();
            ChaState playerState = player.GetComponent<ChaState>();
            float speed = player.GetComponent<ChaState>().speed;


            if (waitUntilSecond > 0){
                yield return new WaitForSeconds(waitUntilSecond);
            }


            Vector3 path = wp.transform.position - player.transform.position;
            Vector2 d = path.normalized;
            playerAction.move(d.x,d.y);
            //Debug.Log(playerState.skills[0].skillName + " Start Moving to " + wp.id);
            while (playerState.isAIControled && path.sqrMagnitude > 0.1f)
            {
                yield return new WaitForSeconds(0.125f);
                path = wp.transform.position - player.transform.position;
                d = path.normalized;
                playerAction.move(d.x,d.y);
                //Debug.Log(playerState.skills[0].skillName + " On Moving to " + wp.id);
            }
            playerAction.stop();
        }

        private bool fading = false;
        protected IEnumerator SlowlyFadingScreen(bool toBlack, float waitUntilSecond=0.0f, float fadeSpeed=2.0f){
            if (cameraMask == null){
                cameraMask = playerCamera.GetComponentInChildren<SpriteRenderer>();
            }
            if (waitUntilSecond > 0){
                yield return new WaitForSeconds(waitUntilSecond);
            }
            while (fading) {
                 yield return new WaitForSeconds(0.125f);
            }
            float fadeDuration = 0.0625f;
            fading = true;
            if(toBlack){
                while (cameraMask.color.a < 0.98) {
                    cameraMask.color = Color.Lerp(cameraMask.color, Color.black, fadeSpeed*fadeDuration);
                    yield return new WaitForSeconds(fadeDuration);
                }
                cameraMask.color = Color.black;
            }else{
                while (cameraMask.color.a > 0.02) {
                    cameraMask.color = Color.Lerp(cameraMask.color, Color.clear, fadeSpeed*fadeDuration);
                    yield return new WaitForSeconds(fadeDuration);
                }
                cameraMask.color = Color.clear;
            }
            fading = false;
        }


        protected IEnumerator enableObject(GameObject gobj, float waitUntilSecond=0.0f){
            yield return new WaitForSeconds(waitUntilSecond);
            gobj.SetActive(true);
        }
        // add dialogues into queue
        protected void PushDialogue(string chaName, string chaWords){
            dialogueLines.Enqueue(new KeyValuePair<string,string>(chaName,chaWords));
        }
        // start dialogue
        // Seems No use.....
        protected void RaiseDialogue(){
            dialogueManager.dialogueLines = dialogueLines;
        }

    }

namespace LevelEvents
{

    public class LevelEventTrigger : UnityEngine.Object {

        public string Description {get; private set;}
        public int id {get; private set;}
        public bool loop {get; private set;}
        public bool hasTriggered {get; private set;}

        private static int LevelEventTriggerNum = 0;
        private Func<bool> Condition;
        private Action Consequence;
        private List<int> reqEvents;

        public LevelEventTrigger(Func<bool> Condition, Action Consequence, string Description, bool loop=false){
            this.Condition = Condition;
            this.Consequence = Consequence;
            this.Description = Description;
            this.id = LevelEventTriggerNum;
            LevelEventTriggerNum += 1;
            this.loop = loop;
            hasTriggered = false;
            reqEvents = new List<int>();
        }

        public bool checkCondition(){
            if(loop){
                return Condition();
            }else{
                if(hasTriggered){
                    return false;
                }else{
                    bool ret = Condition();
                    if(ret){
                        hasTriggered = true;
                    }
                    return ret;
                }
            }
        }


        public void do_Consequence(){
            Consequence();
        }
    }

}
