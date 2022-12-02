using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.Player;

namespace Necropanda.Interfaces
{
    public class PauseInterface : MonoBehaviour
    {
        public PlayerController player;
        public GameObject ConfirmationScreen;
        public GameObject SettingsScreen;
        public GameObject Pausemenu;
        public GameObject AchievementScreen;

        void Start()
        {
            Pausemenu.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                //open pause menu
                if(Pausemenu.activeSelf == true)
                {
                    player.paused = true;
                    //unlock cursor, unpause timescale

                }        

                //close pause menu
                else if (Pausemenu.activeSelf == false)
                {
                    player.paused = false;
                    //lock cursor to centre, pause timescale

                }
            }

        }

        public void Resume()
        {
            Debug.Log("resuming");
            Pausemenu.SetActive(false);
            // Cursor.visible = false;
            // Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }

        public void Settings()
        {
            Pausemenu.SetActive(false);
            SettingsScreen.SetActive(true);
        }

        public void Help()
        {

        }

        public void Achievements()
        {
            AchievementScreen.SetActive(true);
            Pausemenu.SetActive(false);
        }

        public void SaveGame()
        {

        }

        public void QuitGame()
        {
            ConfirmationScreen.SetActive(true);
        }
    }
}