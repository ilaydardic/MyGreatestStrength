using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class NpcStatsHolder : MonoBehaviour
{
    
    // The dialogue runner we want to load the program into
    private DialogueRunner _dialogueRunner;
    
    //The name a.k.a Title of this npc Yarn script
    [SerializeField] private string yarnScriptTitle;

    void Start ()
    {
        _dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        if (_dialogueRunner != null)
        {
            _dialogueRunner.StartDialogue(yarnScriptTitle);
        }
    }

    private void FixedUpdate()
    {
        if (_dialogueRunner && !_dialogueRunner.IsDialogueRunning )
        {
            Destroy(this.gameObject);
        }
    }

 
}
