using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Necropanda.Player;
using UnityEditor;


namespace Necropanda.Interfaces
{
    public class HUDInterface : MonoBehaviour
    {
        public GameObject pauseMenu;
        public PlayerController player;
        private JournalMainCode JournalCode;
        public GameObject mainHUD;
        public GameObject Inventory;
        public GameObject Journal;
        public GameObject Settings;
        public GameObject Credits;

        public static HUDInterface instance;

        public bool gameIsPaused;

        // Start is called before the first frame update
        void Start()
        {
            mainHUD.SetActive(true);
            pauseMenu.SetActive(false);
            JournalCode = Journal.GetComponentInChildren<JournalMainCode>(true);
            QuestMenuUpdater updater = GetComponentInChildren<QuestMenuUpdater>(true);

            if (updater != null)
            {
                updater.Setup();
            }

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (GeneralDialogueLogic.instance.inDialogue) return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(pauseMenu.activeSelf == false  && Time.timeScale == 1)
                {
                    PauseGame();
                }
                else if (pauseMenu.activeSelf == false && Settings.activeSelf == true || Credits.activeSelf == true)
                {
                    Settings.SetActive(false);
                    Credits.SetActive(false);
                    PauseGame();
                }
                else if (pauseMenu.activeSelf == true && Time.timeScale == 0)
                {
                    ResumeGame();
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {

            }
        }

        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            mainHUD.SetActive(false);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            player.paused = true;
            gameIsPaused = true;
        }
        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            mainHUD.SetActive(true);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.paused = false;
            gameIsPaused = false;
        }
    }
}