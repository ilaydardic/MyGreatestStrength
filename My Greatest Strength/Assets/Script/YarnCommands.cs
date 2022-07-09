using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    public class Choices
    {
        public string Gratitude;
        public string Bravery;
        public string Curiosity;
        public string Prudence;
        public string Teamwork;
        public string Kindness;
    }

    public Choices choices = new Choices();

    public GameObject textBackground;
    public Sprite[] setOfSprites;

    [YarnCommand("save")]
    public void Save(string choiceName, int choiceAmount)
    {
        switch (choiceName)
        {
            case "gratitude":
                choices.Gratitude += choiceAmount;
                break;
            case "bravery":
                choices.Bravery += choiceAmount;
                break;
            case "curiosity":
                choices.Curiosity += choiceAmount;
                break;
            case "prudence":
                choices.Prudence += choiceAmount;
                break;
            case "teamwork":
                choices.Teamwork += choiceAmount;
                break;
            case "kindness":
                choices.Kindness += choiceAmount;
                break;
            default:
                throw new Exception("Error, choice does not exist!");
        }

        StartCoroutine(fillCup());
    }

    private IEnumerator fillCup()
    {
        yield return new WaitForSeconds(2.0f);
    }

    [YarnCommand("moodBoard")]
    public void changeMoodBoard(string mood)
    {
        if (mood == "angry")
        {
            //textBackground.GetComponent<SpriteRenderer>().sprite = setOfSprites[0];
            Debug.Log("mood 0");
        }
        else if(mood == "happy")
        {
            //textBackground.GetComponent<SpriteRenderer>().sprite = setOfSprites[1];
            Debug.Log("mood 1");
        }
        else
        {
            Debug.LogWarning("Wrong mood for text background was written!");
        }
    }
    
    private void OnApplicationQuit()
    {
        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ChoicesScoreboard.txt"))
        {
            string json = JsonUtility.ToJson(choices);
            File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ChoicesScoreboard.txt", DateTime.Now + " " + json + "\n");
        }
        else
        {
            string json = JsonUtility.ToJson(choices);
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ChoicesScoreboard.txt",  DateTime.Now + " " + json + "\n");
        }
    }
    
}
