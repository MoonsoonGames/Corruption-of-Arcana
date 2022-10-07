using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreenInterface : MonoBehaviour
{
    public GameObject SettingsMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void NewGame()
    {
        //Reset loadsettings/progress
        //load game
    }

    public void LoadGame()
    {
        //locate save file
        //set load settings/progress
        //load game
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
