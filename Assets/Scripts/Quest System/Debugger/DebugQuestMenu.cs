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
    public class DebugQuestMenu : MonoBehaviour
    {
        public GameObject menu;

        private void Start()
        {
            menu.SetActive(false);
        }

        private void Update()
        {
            if (menu == null) { return; }

            if (Input.GetKeyDown("p"))
            {
                menu.SetActive(!menu.activeSelf);
            }
        }
    }
}
