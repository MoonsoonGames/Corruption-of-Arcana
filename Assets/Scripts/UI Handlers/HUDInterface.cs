using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDInterface : MonoBehaviour
{
    public GameObject SettingsMenu;
    public bool SettingsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SettingsOpen == false)
            {
                Debug.Log("Open Settings");
                SettingsMenu.SetActive(true);
                SettingsOpen = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }        

            else if (SettingsOpen == true)
            {
                Debug.Log("Close Settings");
                SettingsMenu.SetActive(false);
                SettingsOpen = false;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 1;
            }
        }
    }
}
