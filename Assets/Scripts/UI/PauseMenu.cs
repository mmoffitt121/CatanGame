using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void UnPause()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
