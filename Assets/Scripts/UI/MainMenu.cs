using Catan.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.UI
{
    public class MainMenu : MonoBehaviour
    {
        public MainMenuCameraControl cam;

        public GameObject mainMenu;
        public GameObject boardSettings;
        public GameObject playerSettings;
        public GameObject options;

        public void ToBoardSettings()
        {
            UpdateUI(MenuState.BoardSettings);
            cam.ToBoardSettings();
        }

        public void ToPlayerSettings()
        {
            UpdateUI(MenuState.PlayerSettings);
            cam.ToPlayerSettings();
        }

        public void ToMainMenu()
        {
            UpdateUI(MenuState.MainMenu);
            cam.ToMainMenu();
        }

        public void ToOptions()
        {
            UpdateUI(MenuState.Settings);
            cam.ToOptions();
        }

        public void ToAITest()
        {
            DisableUI();
            cam.ToAITest();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void UpdateUI(MenuState state)
        {
            DisableUI();

            switch (state)
            {
                case MenuState.MainMenu:
                    mainMenu.SetActive(true);
                    break;
                case MenuState.BoardSettings:
                    boardSettings.SetActive(true);
                    break;
                case MenuState.PlayerSettings:
                    playerSettings.SetActive(true);
                    break;
                case MenuState.Settings:
                    options.SetActive(true);
                    break;
                default:
                    mainMenu.SetActive(true);
                    break;
            }
        }

        public void DisableUI()
        {
            mainMenu.SetActive(false);
            boardSettings.SetActive(false);
            playerSettings.SetActive(false);
            options.SetActive(false);
        }

        public enum MenuState
        { 
            MainMenu,
            BoardSettings,
            PlayerSettings,
            Settings,
            AITest
        }
    }
}
