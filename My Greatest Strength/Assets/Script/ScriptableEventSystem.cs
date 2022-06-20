using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScriptableEventSystem", order = 1)]
public class ScriptableEventSystem : ScriptableObject
{
    public string eventNameToStart;
    [System.Serializable]
    public class VariableHolder
    {

        public GameObject Npc;
        public string NpcName;

    }
    public List<VariableHolder> Variables = new List<VariableHolder>();


}


/*
            Just because 
 
    [CustomEditor(typeof(ScriptableEventSystem))]

    public class MyScriptableEventSystemEditor : Editor
    {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        ScriptableEventSystem script = target as ScriptableEventSystem;

        if (script.anotherNpc)
        {
            script.secondNpc = EditorGUILayout.ObjectField("Npc Prefab", script.secondNpc, typeof(GameObject), true) as GameObject;
            script.npcDialogueTitle2 = EditorGUILayout.TextField("Npc Dialogue Title", script.npcDialogueTitle2);
        }
    }
    }
*/