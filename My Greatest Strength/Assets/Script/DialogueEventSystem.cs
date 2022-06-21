using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Yarn;
using Yarn.Unity;

public class DialogueEventSystem : MonoBehaviour
{

    [System.Serializable]
    public class PositionHolder
    {
        public GameObject npc;
        public Transform npcInitialPosition;
        public Transform npcFinalPosition;
    }
    //controls the speed of the npc movement during animation
    public float animationMovementSpeed; 
    
    public List<PositionHolder> npcPositionToScreen = new List<PositionHolder>();

    [Tooltip("list of npc, The event order is based on this list [1st event = 1st npc]")] 
    [SerializeField] private List<ScriptableEventSystem> eventQueue;

    [SerializeField] private GameObject lemonadeJar;

    public new Camera camera;
    private bool MouseOnLemonade { get; set;}
    private bool LemonadeJarPressed { get; set; }
    
    // The dialogue runner we want to load the program into
    private DialogueRunner _dialogueRunner;
    //Keeping track of the character name
    public TextMeshProUGUI characterName;
    //check previous speaker
    private string _previousSpeaker;
    //bool to avoid async func
    private bool _callItOnce;
    //place holders to destroy 
    private GameObject[] spawnHolder = {null, null};
   
    // Start is called before the first frame update
    void Start()
    {
        //Defining stuff
        _dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        MouseOnLemonade = false;
    }

    // Update is called once per frame
    void Update()
    {
        //keep track of mouse over lemonade
        if (!_dialogueRunner.IsDialogueRunning)
        {
           CheckForMouseOnJar(); 
        }
    }
    
    void CheckForMouseOnJar()
    {
        //Simple raycast, once find the lemonade Jar. do something
        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit == lemonadeJar)
        {
            MouseOnLemonade = true;
            lemonadeJar.GetComponent<SpriteRenderer>().color = Color.red;
            if (Input.GetMouseButtonDown(0) && MouseOnLemonade)
            {
                LemonadeJarPressed = true;
                CallForEvent();
                lemonadeJar.GetComponent<SpriteRenderer>().color = Color.yellow;
                MouseOnLemonade = false;
            }
        }
        else
        {
            MouseOnLemonade = false;
            lemonadeJar.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        
    }

    void CallForEvent()
    {
        //Just a start conversation + spawn npc.
        if (!_dialogueRunner.IsDialogueRunning && eventQueue.First().eventNameToStart != null)
        {
            _dialogueRunner.StartDialogue(eventQueue.First().eventNameToStart);
            CheckNpcConversation();
            _callItOnce = true;
        }
    }
    public void BoolNextDialogueChart()
    {
        //being called by continue button. I prefer this than a async call. xD
        _callItOnce = true;
    }
    public void CheckNpcConversation()
    {
        //This is being call on the Line view (event function character typed . That is called a few times ... )
        
        //Check for new character name. If true -> spawn it where's available .
        if (_callItOnce)
        {
            //No npc yet on this holder. Spawn it
            if (npcPositionToScreen[0].npc == null)
            {
                for(int i = 0; i < eventQueue.First().Variables.Count; i++)
                {
                    if (characterName.text == eventQueue.First().Variables[i].Npc.name)
                    {
                        SpawnDestroyNpc(i, 0);
                        break;
                    }
                }
            }
            //npc on previous holder but not this one. Spawn it
            else if(npcPositionToScreen[1].npc == null)
            {
                for(int i = 0; i < eventQueue.First().Variables.Count; i++)
                {
                    if (characterName.text == eventQueue.First().Variables[i].Npc.name)
                    {
                        SpawnDestroyNpc(i, 1);
                        break;
                    }
                }
            }
            //npc everywhere OMG. Get the first that spoke so it can have the previous + new npc
            else
            {
                if (npcPositionToScreen[0].npc.name != characterName.text &&
                    npcPositionToScreen[1].npc.name != characterName.text)
                {
                    for (int i = 0; i < eventQueue.First().Variables.Count; i++)
                    {
                        if (characterName.text == eventQueue.First().Variables[i].NpcName)
                        {
                            if (npcPositionToScreen[0].npc.name == _previousSpeaker)
                            {
                                SpawnDestroyNpc(i, 1);
                            }
                            else
                            {
                                SpawnDestroyNpc(i, 0);
                            }
                        }
                    }
                }
            }
        }
        _callItOnce = false;
        _previousSpeaker = characterName.text;
    }

    public void EndEvent()
    {
        Destroy(spawnHolder[0]);
        Destroy(spawnHolder[1]);
        eventQueue.Remove(eventQueue.First());
    }

    void SpawnDestroyNpc(int forInt, int leftOrRight)
    {
        // 0 is left | 1 is right (Just because spawnAholder
        if (spawnHolder[leftOrRight] != null)
        {
            //MoveNpcAtSpawn(leftOrRight, true);
            Destroy(spawnHolder[leftOrRight]);
        }
        spawnHolder[leftOrRight] = Instantiate(eventQueue.First().Variables[forInt].Npc, npcPositionToScreen[leftOrRight].npcFinalPosition);
        npcPositionToScreen[leftOrRight].npc = eventQueue.First().Variables[forInt].Npc;
        //MoveNpcAtSpawn(leftOrRight, false);
    }

    void MoveNpcAtSpawn(int leftOrRight, bool oldNpc)
    {
        //check for old character 
        if (oldNpc)
        {
            spawnHolder[leftOrRight].transform.position = Vector2.Lerp(npcPositionToScreen[leftOrRight].npcFinalPosition.position,
                npcPositionToScreen[leftOrRight].npcInitialPosition.position, animationMovementSpeed);
        }
        else
        {
            npcPositionToScreen[leftOrRight].npc.transform.position = Vector2.Lerp(npcPositionToScreen[leftOrRight].npc.transform.position,
                npcPositionToScreen[leftOrRight].npcFinalPosition.position, animationMovementSpeed * Time.deltaTime); 
        }
        
    }
    
}
