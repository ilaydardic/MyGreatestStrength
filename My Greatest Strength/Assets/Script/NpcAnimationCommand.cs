using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NpcAnimationCommand : MonoBehaviour
{
    /* There were other ways to do that. but the delivery date was near and brain doesnt work.
     *  ways = Animator + Blend tree
     *  Mainbody has access to both animations.
     */
    public GameObject npcBody;
    private Animation bodyAnim;
    public GameObject npcFace;
    private Animation faceAnim;
    private void Start()
    {
        bodyAnim = npcBody.GetComponent<Animation>();
        faceAnim = npcFace.GetComponent<Animation>();
    }

    [YarnCommand("animation")]
    public void ChangeAnimation(string animationClipName)
    {
        //Npcs first name + the animation. It could be only the animations name but then it would be a bit confusing inside assets folder
        string name = npcBody.name.Split(new char[] {'('})[0];
        if (bodyAnim.GetClip(name + animationClipName) == null)
        {
            Debug.LogWarning("This expression doesnt exist!");
        }
        else
        {
            bodyAnim.Play(name + animationClipName);
        }
    }
    [YarnCommand("expression")]
    public void ChangeExpression(string expressionClipName)
    {
        //Npcs first name + the animation. It could be only the animations name but then it would be a bit confusing inside assets folder
        string name = npcBody.name.Split(new char[] {'('})[0];
        if (faceAnim.GetClip(name + expressionClipName) == null)
        {
            Debug.LogWarning("This expression doesnt exist!");
        }
        else
        {
            faceAnim.Play(name + expressionClipName);
        }
        
    }
}
