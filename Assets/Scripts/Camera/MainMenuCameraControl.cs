/// AUTHOR: Matthew Moffitt
/// FILENAME: MainMenuCameraControl.cs
/// SPECIFICATION: File that controls the camera in the main menu
/// FOR: CS 3368 Introduction to Artificial Intelligence Section 001

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.UI;

namespace Catan.Camera
{
    /// <summary>
    /// Class that controls the position of the camera in the main menu
    /// </summary>
    public class MainMenuCameraControl : MonoBehaviour
    {
        /// <summary>
        /// Animator that controls position of the camera
        /// </summary>
        public Animator animator;
        /// <summary>
        /// Value that controls the state of the camera
        /// </summary>
        public MainMenu.MenuState state;

        // Functions that change the state of the camera.

        public void ToBoardSettings()
        {
            state = MainMenu.MenuState.BoardSettings;
            UpdateAnimator();
        }

        public void ToPlayerSettings()
        {
            state = MainMenu.MenuState.PlayerSettings;
            UpdateAnimator();
        }

        public void ToMainMenu()
        {
            state = MainMenu.MenuState.MainMenu;
            UpdateAnimator();
        }

        public void ToOptions()
        {
            state = MainMenu.MenuState.Settings;
            UpdateAnimator();
        }

        public void ToAITest()
        {
            state = MainMenu.MenuState.AITest;
            UpdateAnimator();
        }

        public void UpdateAnimator()
        {
            animator.SetInteger("State", (int)state);
        }
    }
}
