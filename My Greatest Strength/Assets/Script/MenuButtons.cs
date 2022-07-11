using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(WaitForSound());

        IEnumerator WaitForSound()
        {
            yield return new WaitForSeconds(0.7f);

            SceneManager.LoadScene("BuildScene");
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitForSound());

        IEnumerator WaitForSound()
        {
            yield return new WaitForSeconds(0.7f);

            SceneManager.LoadScene("Menus1Scene");
        }
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitForSound());

        IEnumerator WaitForSound()
        {
            yield return new WaitForSeconds(0.7f);

            Application.Quit();
        }
    }
}
