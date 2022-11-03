using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NecroPanda.Player;

public class PauseInterface : MonoBehaviour
{
    public PlayerController player;
    public GameObject ConfirmationScreen;
    public GameObject SettingsScreen;
    public GameObject Pausemenu;

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
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }        

            //close pause menu
            else
            {
                player.paused = false;
                //lock cursor to centre, pause timescale
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void Resume()
    {
        Pausemenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    public void SaveGame()
    {

    }

    public void QuitGame()
    {
        ConfirmationScreen.SetActive(true);
    }
}
