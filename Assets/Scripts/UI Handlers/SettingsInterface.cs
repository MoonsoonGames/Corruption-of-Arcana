using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInterface : MonoBehaviour
{
    public GameObject SettingsScreen;
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SEVolume;
    public Slider DialogueVolume; //This might be scraped due to timeframe
    public bool SettingsOpen = false;

    public void Settings()
    {
        //open settings screen
        SettingsScreen.SetActive(true);
        SettingsOpen = true;
    }

    void Start()
    {
        SettingsOpen = false;
        SettingsScreen.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SettingsOpen == true)
            {
                Debug.Log("Close Settings");
                SettingsScreen.SetActive(false);
                SettingsOpen = false;
            }    
        }
    }
}
