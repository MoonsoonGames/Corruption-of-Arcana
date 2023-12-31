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
    public class EnemyManager : TeamManager
    {
        public Character player;
        public EnemySpawner[] spawners;
        public EnemyQueue enemyQueue { get; private set; }

        protected override void Start()
        {
            enemyQueue = GameObject.FindObjectOfType<EnemyQueue>();
            enemyQueue.Setup();
            base.Start();
        }

        public override void StartTurn()
        {
            CheckKilled();
            Invoke("SpawnEnemies", 0.02f);
            Invoke("ActivateTurns", 0.03f);
        }

        //Delete once AI has been implemented
        public override void ActivateTurns()
        {
            foreach (Character character in team)
            {
                if (character.GetHealth().GetHealth() > 0)
                {
                    character.StartTurn();
                }
            }
        }

        protected override void Setup()
        {
            base.Setup();
            SpawnEnemies();
        }

        void SpawnEnemies()
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].filled == false)
                {
                    if (LoadCombatManager.instance.enemies.Count != 0)
                    {
                        CharacterStats stats = LoadCombatManager.instance.enemies[0].stats;
                        LoadCombatManager.instance.enemies.RemoveAt(0);

                        if (stats != null)
                        {
                            GameObject enemyRef = Instantiate(stats.characterObject, spawners[i].gameObject.transform) as GameObject;

                            Character enemyCharacter = enemyRef.GetComponent<Character>();
                            enemyCharacter.stats = stats;

                            //Debug.Log("Spawned " + enemyCharacter.stats.characterName + " at spawner " + i);
                            //team.Add(enemyCharacter);
                            spawners[i].SpawnEnemy(enemyCharacter);
                        }
                    }
                }
            }

            if (enemyQueue != null)
                enemyQueue.UpdateUI();
        }

        static int SortByOrder(EnemySpawner s1, EnemySpawner s2)
        {
            return s1.order.CompareTo(s2.order);
        }
    }
}