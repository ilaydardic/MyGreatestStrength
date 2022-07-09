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
        StartCoroutine(WaitForSound());

        IEnumerator WaitForSound()
        {
            SFXSelect.Play();

            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene("BuildScene");
        }
    }

    public void GoToMainMenu()
    {
        StartCoroutine(WaitForSound());

        IEnumerator WaitForSound()
        {
            SFXSelect.Play();

            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene("Menus1Scene");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
