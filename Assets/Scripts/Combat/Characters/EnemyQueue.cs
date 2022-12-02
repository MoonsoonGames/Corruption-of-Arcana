using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class EnemyQueue : MonoBehaviour
    {
        LoadCombatManager loadCombatManager;
        public Image enemyImage;
        public TextMeshProUGUI countText;

        public void Setup()
        {
            loadCombatManager = LoadCombatManager.instance;
            loadCombatManager.queue = this;
        }

        public void UpdateUI()
        {
            countText.text = loadCombatManager.enemies.Count.ToString();

            if (loadCombatManager.enemies.Count > 0)
            {
                Object nextEnemy = loadCombatManager.enemies[0];
                GameObject objectRef = Instantiate(nextEnemy, gameObject.transform) as GameObject;
                Character character = objectRef.GetComponent<Character>();
                Sprite newSprite = character.stats.characterSprite;
                enemyImage.sprite = newSprite;
                Color color = enemyImage.color;
                color.a = 255;
                enemyImage.color = color;
                Destroy(objectRef);
            }
            else
            {
                enemyImage.sprite = null;
                Color color = enemyImage.color;
                color.a = 0;
                enemyImage.color = color;
            }
        }
    }
}