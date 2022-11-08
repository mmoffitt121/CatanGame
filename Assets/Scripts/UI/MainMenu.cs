using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Catan.Camera;
using Catan.Settings;
using Catan.Players;

namespace Catan.UI
{
    public class MainMenu : MonoBehaviour
    {
        public MainMenuCameraControl cam;

        public GameObject mainMenu;
        public GameObject boardSettings;
        public GameObject playerSettings;
        public GameObject options;

        public void ToBoardSettings(bool testing)
        {
            GameSettings.testing = testing;
            GameSettings.quickrolling = testing;
            UpdateUI(MenuState.BoardSettings);
            cam.ToBoardSettings();
        }

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

        public void StartGame()
        {
            if (SetPlayers())
            {
                SceneManager.LoadScene("Game");
            }
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public bool SetPlayers()
        {
            Player p0 = GameObject.Find("PlayerPanel0").GetComponent<PlayerSettings>().GetPlayer();
            Player p1 = GameObject.Find("PlayerPanel1").GetComponent<PlayerSettings>().GetPlayer();
            Player p2 = GameObject.Find("PlayerPanel2").GetComponent<PlayerSettings>().GetPlayer();
            Player p3 = GameObject.Find("PlayerPanel3").GetComponent<PlayerSettings>().GetPlayer();
            Player p4 = GameObject.Find("PlayerPanel4").GetComponent<PlayerSettings>().GetPlayer();
            Player p5 = GameObject.Find("PlayerPanel5").GetComponent<PlayerSettings>().GetPlayer();

            List<Player> players = new List<Player>();

            if (p0 != null) players.Add(p0);
            if (p1 != null) players.Add(p1);
            if (p2 != null) players.Add(p2);
            if (p3 != null) players.Add(p3);
            if (p4 != null) players.Add(p4);
            if (p5 != null) players.Add(p5);

            if (players.Count < 3)
            {
                return false;
            }

            GameSettings.players = players.ToArray();
            return true;
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
