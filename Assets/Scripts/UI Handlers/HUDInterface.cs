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
        public GameObject mainHUD;
        public static bool gameIsPaused;

        // Start is called before the first frame update
        void Start()
        {
            mainHUD.SetActive(true);
            Pausemenu.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameIsPaused = !gameIsPaused;
                PauseGame();
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