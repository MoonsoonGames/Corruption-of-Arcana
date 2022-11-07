using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class AchievementInterface : MonoBehaviour
    {
        public GameObject AchievementScreen;
        public GameObject PauseMenu;
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (AchievementScreen.activeSelf == true)
                {
                    AchievementScreen.SetActive(false);
                    PauseMenu.SetActive(true);
                }
            }
        }
        
        public void Close()
        {
            AchievementScreen.SetActive(false);
            PauseMenu.SetActive(true);
        }
    }
}
