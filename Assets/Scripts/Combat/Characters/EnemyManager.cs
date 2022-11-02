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
        EnemySpawner[] spawners;
        EnemyQueue enemyQueue;

        protected override void Start()
        {
            enemyQueue = GetComponentInChildren<EnemyQueue>();
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
                    //In future, determine target depending on spell so it can cast support spells on allies/self
                    CombatHelperFunctions.SpellInstance newSpellInstance = new CombatHelperFunctions.SpellInstance();
                    newSpellInstance.SetSpellInstance(character.PrepareSpell(), player, character);

                    timeline.AddSpellInstance(newSpellInstance);
                }
            }
        }

        protected override void Setup()
        {
            base.Setup();

            //spawn enemies from load settings
            spawners = GameObject.FindObjectsOfType<EnemySpawner>();

            List<EnemySpawner> spawnerList = new List<EnemySpawner>();

            foreach (EnemySpawner spawner in spawners)
            {
                spawnerList.Add(spawner);
            }

            spawnerList.Sort(SortByOrder);

            for (int i = 0; i < spawnerList.Count; i++)
            {
                spawners[i] = spawnerList[i];
            }

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
                        Object enemyObject = LoadCombatManager.instance.enemies[0];
                        LoadCombatManager.instance.enemies.RemoveAt(0);

                        if (enemyObject != null)
                        {
                            GameObject enemyRef = Instantiate(enemyObject, spawners[i].gameObject.transform) as GameObject;

                            Character enemyCharacter = enemyRef.GetComponent<Character>();

                            Debug.Log("Spawned " + enemyCharacter.characterName + " at spawner " + i);
                            //team.Add(enemyCharacter);
                            spawners[i].filled = true;
                            enemyCharacter.spawner = spawners[i];
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