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
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene("BuildScene");
        }
    }

    public void GoToMainMenu()
    {
        StartCoroutine(WaitForSound());

        IEnumerator WaitForSound()
        {
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene("Menus1Scene");
        }
    }

    public void ExitGame()
    {
        StartCoroutine(WaitForSound());

        IEnumerator WaitForSound()
        {
            yield return new WaitForSeconds(1f);

            Application.Quit();
        }
    }
}
