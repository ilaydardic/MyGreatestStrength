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
    private GameObject spawnAHolder;
    private GameObject spawnBHolder;

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
                        spawnAHolder = Instantiate(eventQueue.First().Variables[i].Npc, npcPositionToScreen[0].npcInitialPosition);
                        npcPositionToScreen[0].npc = eventQueue.First().Variables[i].Npc;
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
                        spawnBHolder = Instantiate(eventQueue.First().Variables[i].Npc, npcPositionToScreen[1].npcInitialPosition);
                        npcPositionToScreen[1].npc = eventQueue.First().Variables[i].Npc;
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
                                Destroy(spawnBHolder);
                                spawnBHolder = Instantiate(eventQueue.First().Variables[i].Npc,
                                    npcPositionToScreen[1].npcInitialPosition);
                                npcPositionToScreen[1].npc = eventQueue.First().Variables[i].Npc;
                            }
                            else
                            {
                                Destroy(spawnAHolder);
                                spawnAHolder = Instantiate(eventQueue.First().Variables[i].Npc,
                                    npcPositionToScreen[0].npcInitialPosition);
                                npcPositionToScreen[0].npc = eventQueue.First().Variables[i].Npc;
                            }
                        }
                    }
                }
            }
        }
        _callItOnce = false;
        _previousSpeaker = characterName.text;
    }

    void EndEvent()
    {
        for (int i = 0; i < eventQueue.First().Variables.Count; i++)
        {
            Destroy(eventQueue.First().Variables[i].Npc);
        }
        eventQueue.Remove(eventQueue.First());
    }
    
}
