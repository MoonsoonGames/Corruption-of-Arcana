using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class CombatManager : MonoBehaviour
    {
        public Character player;

        public EnemyManager enemyManager;

        public GameObject victoryScreen;
        public GameObject defeatScreen;

        public static CombatManager instance;

        private void Start()
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void CharacterDied(Character character)
        {
            if (character == player)
            {
                ShowEndScreen(false);
            }
            else
            {
                if (enemyManager.team.Count == 0)
                {
                    ShowEndScreen(true);
                }
            }
        }

        void ShowEndScreen(bool victory)
        {
            victoryScreen.SetActive(victory);
            defeatScreen.SetActive(!victory);
        }
    }
}