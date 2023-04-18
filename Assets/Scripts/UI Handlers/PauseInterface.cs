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
        public GameObject savedText;
        public GameObject CreditsScreen;
        //public GameObject AchievementScreen;


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
        
        public void Credits()
        {
            CreditsScreen.SetActive(true);
        }

        public void SaveGame()
        {
            //Maybe have a visual indicator for this
            savedText.SetActive(true);
            SaveManager.instance.SaveAllData();
            new WaitForSecondsRealtime(2f);
            savedText.SetActive(false);
        }

        public void QuitGame()
        {
            //ConfirmationScreen.SetActive(true);
            Application.Quit();
        }

        public void CloseCredits()
        {
            CreditsScreen.SetActive(false);
        }
    }
}