/// AUTHOR: Matthew Moffitt
/// FILENAME: PauseMenu.cs
/// SPECIFICATION: Controls pause menu
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    // UI objects
    public GameObject pauseMenu;
    public GameObject pauseButton;

    /// <summary>
    /// Opens pause menu
    /// </summary>
    public void Pause()
    {
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
    }

    /// <summary>
    /// Closes pause menu
    /// </summary>
    public void UnPause()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    /// <summary>
    /// Returns to main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
