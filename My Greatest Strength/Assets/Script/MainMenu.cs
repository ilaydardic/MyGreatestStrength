using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [Header("SFX Clips")]
    public AudioSource SFXSelect;

    public void PlayGame()
    {
        SFXSelect.Play();

        if (!SFXSelect.isPlaying)
        {
            SceneManager.LoadScene("BuildScene");
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menus1Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
