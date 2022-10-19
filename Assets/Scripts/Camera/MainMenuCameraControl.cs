using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catan.UI;

namespace Catan.Camera
{
    public class MainMenuCameraControl : MonoBehaviour
    {
        public Animator animator;
        public MainMenu.MenuState state;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Going");
                state = (MainMenu.MenuState)(((int)state + 1)%5);
                UpdateAnimator();
            }
        }

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
