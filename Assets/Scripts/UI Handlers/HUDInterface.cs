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
        public GameObject Journal; //not in use currently
        public GameObject Settings;
        public GameObject Credits;

        public GameObject currentMenu;

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

            #region ESC Key
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                if (Time.timeScale == 1) //Game Running
                {
                    if (pauseMenu.activeSelf == false)
                    {
                        PauseGame();
                    }
                }
                else if (Time.timeScale == 0) //Game Paused
                {
                    if (pauseMenu.activeSelf == false && Settings.activeSelf == true || Credits.activeSelf == true)
                    {
                        Settings.SetActive(false);
                        Credits.SetActive(false);
                        PauseGame();
                    }
                    else if (pauseMenu.activeSelf == true)
                    {
                        ResumeGame();
                    }
                    else if (Inventory.activeSelf == true)
                    {
                        ResumeGame();
                    }
                }
            }
            #endregion

            #region TAB Key
            if (Input.GetKeyDown(KeyCode.Tab) && Time.timeScale == 1)
            {
                if (Inventory.activeSelf == true)
                {
                    Inventory.SetActive(false);
                    ResumeGame();
                }

                if (Inventory.activeSelf == false)
                {
                    PauseGame();
                    pauseMenu.SetActive(false);
                    OpenInventory();
                }
            }
            #endregion

        }

        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            mainHUD.SetActive(false);
            Time.timeScale = 0;
            ReleaseCursor();
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            Inventory.SetActive(false);
            mainHUD.SetActive(true);
            Time.timeScale = 1;
            LockCursor();
        }

        public void OpenInventory()
        {
            pauseMenu.SetActive(false);
            Inventory.SetActive(true);
            ReleaseCursor();
        }

        public void CloseMenu(GameObject currentMenu)
        {
            currentMenu.SetActive(false);
            PauseGame();
        }

        public void CloseSubMenu(GameObject currentSubMenu)
        {
            currentSubMenu.SetActive(false);
            PauseGame();
            OpenInventory();
        }

        #region Cursor States
        public void LockCursor() //Use when returning to playing state
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            player.paused = false;
            gameIsPaused = false;
        }
        public void ReleaseCursor() //Able to use mouse in menus
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            player.paused = true;
            gameIsPaused = true;
        }
        #endregion

    }
}