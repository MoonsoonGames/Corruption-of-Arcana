using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public Image backdropImage;
        public GameObject loadingImage;

        private void Start()
        {
            instance = this;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            loadingImage.SetActive(true);

            Invoke("Setup", 0.1f);
        }

        void Setup()
        {
            loadingImage.SetActive(false);

            if (LoadCombatManager.instance.backdrop != null)
            {
                backdropImage.sprite = LoadCombatManager.instance.backdrop;
                LoadCombatManager.instance.backdrop = null;
            }

            DeckManager.instance.SetupDecks();
        }

        public void CharacterDied(Character character)
        {
            if (LoadCombatManager.instance.enemiesEndCombat.Contains(character.stats))
            {
                LoadCombatManager.instance.enemiesEndCombat.Remove(character.stats);
                if (LoadCombatManager.instance.enemiesEndCombat.Count <= 0)
                {
                    foreach(var enemy in enemyTeamManager.team)
                    {
                        enemy.GetHealth().ChangeHealth(E_DamageTypes.Physical, 99999999, null);
                    }
                    ShowEndScreen(true);
                }
            }

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

        public TeamManager GetCharacterTeam(TeamManager teamManager)
        {
            TeamManager outTeam = null;
            if (teamManager == playerTeamManager)
                outTeam = enemyTeamManager;
            else if (teamManager == enemyTeamManager)
                outTeam = playerTeamManager;
            return outTeam;
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

        public List<Character> GetAllCharacters()
        {
            List<Character> list = new List<Character>();

            foreach (var item in playerTeamManager.team)
            {
                list.Add(item);
            }

            foreach (var item in enemyTeamManager.team)
            {
                list.Add(item);
            }

            return list;
        }
    }
}