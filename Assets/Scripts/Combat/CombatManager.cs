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
        public TeamManager playerTeamManager;
        public TeamManager enemyTeamManager;
        public Character redirectedCharacter;

        public GameObject victoryScreen;
        public GameObject defeatScreen;

        public static CombatManager instance;

        private void Start()
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            Invoke("Setup", 0.1f);
        }

        void Setup()
        {
            DeckManager.instance.SetupDecks();
        }

        public void CharacterDied(Character character)
        {
            if (redirectedCharacter == character)
            {
                redirectedCharacter = null;
            }

            //Debug.Log("Character Killed");
            if (playerTeamManager.team.Contains(character))
            {
                //Debug.Log("Character Killed on player team");
                playerTeamManager.Remove(character);
                if (playerTeamManager.team.Count == 0)
                {
                    ShowEndScreen(false);
                }
            }
            else
            {
                //Debug.Log("Character Killed on enemy team");
                enemyTeamManager.Remove(character);
                if (enemyTeamManager.team.Count + LoadCombatManager.instance.enemies.Count == 0)
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

        public TeamManager GetOpposingTeam(TeamManager teamManager)
        {
            TeamManager outTeam = null;
            if (teamManager == playerTeamManager)
                outTeam = enemyTeamManager;
            else if (teamManager == enemyTeamManager)
                outTeam = playerTeamManager;
            return outTeam;
        }
    }
}