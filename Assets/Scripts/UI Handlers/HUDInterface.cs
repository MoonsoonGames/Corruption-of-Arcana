using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Necropanda.Player;

public class HUDInterface : MonoBehaviour
{
    public GameObject Pausemenu;
    public PlayerController player;
    public GameObject HUD;

    // Start is called before the first frame update
    void Start()
    {
        HUD.SetActive(true);
        Pausemenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //open pause menu
            if(player.paused == false)
            {
                Pausemenu.SetActive(true);
                HUD.SetActive(false);
                player.paused = true;
                Time.timeScale = 0;
            }        

            //close pause menu
            else if (player.paused == true)
            {
                Pausemenu.SetActive(false);
                HUD.SetActive(true);
                player.paused = false;
                Time.timeScale = 1;
            }
        }
    }
}
