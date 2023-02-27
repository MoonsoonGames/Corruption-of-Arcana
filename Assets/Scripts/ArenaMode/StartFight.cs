using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class StartFight : MonoBehaviour
    {
        public List<Quest> progressQuest;
        public List<CharacterStats> enemies;

        public TextMeshProUGUI text;
        public string overideDescription;

        private void Start()
        {
            if (text == null) return;

            string description = "";

            if (overideDescription == "")
            {
                Dictionary<CharacterStats, int> enemyDictionary = new Dictionary<CharacterStats, int>();

                foreach (var item in enemies)
                {
                    if (enemyDictionary.ContainsKey(item))
                    {
                        enemyDictionary[item] = enemyDictionary[item] + 1;
                    }
                    else
                    {
                        enemyDictionary.Add(item, 1);
                    }
                }

                foreach (var item in enemyDictionary)
                {
                    string enemyName = item.Key.characterName;

                    description += item.Value + " " + enemyName + ", ";
                }
            }
            else
            {
                description = overideDescription;
            }

            text.text = description;
        }

        public void FightButton()
        {
            string sceneString = SceneManager.GetActiveScene().name;
            E_Scenes lastScene = HelperFunctions.StringToSceneEnum(sceneString);
            LoadCombatManager.instance.LoadCombat(null, lastScene, enemies, progressQuest);
        }

        public void StartFightFromLevel()
        {
            GameObject player = GameObject.FindObjectOfType<Player.PlayerController>().gameObject;

            string sceneString = SceneManager.GetActiveScene().name;
            E_Scenes lastScene = HelperFunctions.StringToSceneEnum(sceneString);
            LoadCombatManager.instance.LoadCombat(player, lastScene, enemies, progressQuest);
        }
    }
}
