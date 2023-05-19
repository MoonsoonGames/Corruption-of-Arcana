using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Necropanda.Player;


namespace Necropanda.Interfaces
{
    public class HUDInterface : MonoBehaviour
    {
        public GameObject Pausemenu;
        public PlayerController player;
        private JournalMainCode JournalCode;
        public GameObject mainHUD;
        public GameObject Inventory;
        public GameObject Journal;
        public GameObject Settings;

        public bool gameIsPaused;

        // Start is called before the first frame update
        void Start()
        {
            mainHUD.SetActive(true);
            Pausemenu.SetActive(false);
            JournalCode = Journal.GetComponentInChildren<JournalMainCode>(true);
            QuestMenuUpdater updater = GetComponentInChildren<QuestMenuUpdater>(true);

            if (updater != null)
            {
                updater.Setup();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Inventory.activeSelf == true)
                {
                    player.paused = false;
                    Time.timeScale = 1;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Inventory.SetActive(false);
                    mainHUD.SetActive(true);
                }
                else
                {
                    gameIsPaused = !gameIsPaused;
                    PauseGame();
                }

                if (Journal.activeSelf == true)
                {
                    Journal.SetActive(false);
                    Inventory.SetActive(true);
                    gameIsPaused = true;
                    Pausemenu.SetActive(false);
                    JournalCode.BestiarySection.SetActive(false);
                }

                if (Settings.activeSelf == true)
                {
                    Settings.SetActive(false);
                    PauseGame();
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (gameIsPaused == false)
                {
                    Inventory.SetActive(true);
                    mainHUD.SetActive(false);
                    player.paused = true;
                    Time.timeScale = 0;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }

        }

        void PauseGame()
        {
            if (gameIsPaused)
            {
                Pausemenu.SetActive(true);
                mainHUD.SetActive(false);
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                player.paused = true;
            }

            else
            {
                Pausemenu.SetActive(false);
                mainHUD.SetActive(true);
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                player.paused = false;
            }
        }
    }
}