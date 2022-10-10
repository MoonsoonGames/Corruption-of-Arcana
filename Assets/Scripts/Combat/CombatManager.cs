using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void CharacterDied(Character character)
    {
        if (character == player)
        {
            ShowEndScreen(false);
        }
        else
        {
            if (enemyManager.enemies.Count == 0)
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
