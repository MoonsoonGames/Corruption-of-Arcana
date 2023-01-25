using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.Player;

namespace Necropanda.Interfaces
{
    public class PauseInterface : MonoBehaviour
    {
        //public GameObject ConfirmationScreen;
        public GameObject SettingsScreen;
        public GameObject MainHUD;
        public GameObject Pausemenu;
        public PlayerController player;
        //public GameObject AchievementScreen;

        private void Start()
        {

        }

        public void Resume()
        {
            Debug.Log("resuming");
            Pausemenu.SetActive(false);
            MainHUD.SetActive(true);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.paused = false;
        }

        public void Settings()
        {
            SettingsScreen.SetActive(true);
            Pausemenu.SetActive(false);
        }

        public void Help()
        {

        }

        public void Achievements()
        {
            //AchievementScreen.SetActive(true);
            //Pausemenu.SetActive(false);
        }

        public void SaveGame()
        {

        }

        public void QuitGame()
        {
            //ConfirmationScreen.SetActive(true);
            Application.Quit();
        }
    }
}