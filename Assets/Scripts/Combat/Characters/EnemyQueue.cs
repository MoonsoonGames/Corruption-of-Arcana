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
            countText.text = "x " + loadCombatManager.enemies.Count.ToString();

            if (loadCombatManager.enemies.Count > 0)
            {


                CharacterStats nextEnemy = loadCombatManager.enemies[0].stats;
                Sprite newSprite = nextEnemy.characterSprite;
                enemyImage.sprite = newSprite;
                Color color = enemyImage.color;
                color.a = 255;
                enemyImage.color = color;
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