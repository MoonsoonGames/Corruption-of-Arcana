using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        public List<CharacterStats> enemies;

        public TextMeshProUGUI text;

        private void Start()
        {
            string description = "";

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

            text.text = description;
        }

        public void FightButton()
        {
            LoadCombatManager.instance.LoadCombat(enemies);
        }
    }
}
