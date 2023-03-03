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
    public class ArenaMode : MonoBehaviour
    {
        public bool inGame = false;
        public GameObject canvas;
        StartFight[] fightButtons;

        private void Start()
        {
            if (inGame)
            {
                fightButtons = GetComponentsInChildren<StartFight>(true);
            }
        }

        public void OpenArena()
        {
            canvas.SetActive(true);
            foreach(var item in fightButtons)
            {
                item.gameObject.SetActive(true);

                item.Check();
            }
        }
    }
}
