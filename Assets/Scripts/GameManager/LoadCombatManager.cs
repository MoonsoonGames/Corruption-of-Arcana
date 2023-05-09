using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.AI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class LoadCombatManager : MonoBehaviour
    {
        #region Singleton
        //Code from last year

        public static LoadCombatManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                gameObject.transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
            interacted = new List<string>();
        }

        public CharacterStats character;

        public E_Scenes combatScene;
        public E_Scenes tutorialScene;

        public E_Scenes lastScene;
        public Vector3 lastPos;
        public Quaternion lastRot;

        public List<string> interacted;

        public float combatRadius = 15f;

        public EnemyQueue queue;
        public List<EnemySpawn> enemies;
        public List<CharacterStats> enemiesEndCombat;
        public List<string> enemyIDs;
        public Sprite backdrop;
        bool loading = false;

        public void LoadCombat(GameObject player, E_Scenes lastScene)
        {
            if (loading) return;
            loading = true;

            //Get enemies within radius of player and save them in a list
            enemies.Clear();
            enemyIDs.Clear();
            EnemyAI[] enemyAI = GameObject.FindObjectsOfType<EnemyAI>();

            List<Quest> quests = new List<Quest>();

            foreach (EnemyAI enemy in enemyAI)
            {
                if (enemy.GetActive() && Vector3.Distance(player.transform.position, enemy.transform.position) < combatRadius)
                {
                    //If enemy is a boss, save them in the first space
                    EnemySpawn enemySpawn = new EnemySpawn();
                    enemySpawn.stats = enemy.enemyStats;
                    enemySpawn.spawner = null;

                    enemies.Insert(enemy.boss ? 0 : enemies.Count, enemySpawn);

                    if (enemy.enemyStats.endCombatOnKill || enemy.endCombatIfKilled)
                    {
                        //If enemy is a boss, save them in the first space
                        enemiesEndCombat.Insert(enemy.boss ? 0 : enemiesEndCombat.Count, enemy.enemyStats);
                    }

                    if (enemy.GetComponentInChildren<LoadCombat>().progressQuests.Length > 0)
                    {
                        List<Quest> enemyQuests = new List<Quest>();

                        foreach (Quest quest in enemy.GetComponentInChildren<LoadCombat>().progressQuests)
                        {
                            quests.Add(quest);
                        }
                    }

                    Interactable.Interactable interactable = enemy.GetComponent<Interactable.Interactable>();
                    enemyIDs.Add(interactable.interactID);
                }
            }

            if (enemies.Count <= 0) return;

            foreach(var item in quests)
            {
                Debug.Log(item.questName);
            }

            progressQuestUponCombatVictory = quests;

            //Saving last scene
            if (lastScene != E_Scenes.Null && player != null)
            {
                lastPos = player.transform.position;
                lastRot = player.transform.rotation;
            }

            Debug.Log("Interacted - Load Combat");
            loading = false;
            LoadingScene.instance.LoadScene(combatScene, lastScene, false);
        }

        public void LoadCombat(GameObject player, E_Scenes lastScene, List<CharacterStats> newEnemies, List<Quest> quests)
        {
            if (loading || newEnemies.Count == 0) return;
            loading = true;

            //Get enemies within radius of player and save them in a list
            enemies.Clear();

            List<EnemySpawn> enemySpawnList = new List<EnemySpawn>();

            foreach (var item in newEnemies)
            {
                EnemySpawn enemySpawn = new EnemySpawn();
                enemySpawn.stats = item;
                enemySpawn.spawner = null;
            }

            enemies = enemySpawnList;
            enemyIDs.Clear();

            //Saving last scene
            if (lastScene != E_Scenes.Null && player != null)
            {
                lastPos = player.transform.position;
                lastRot = player.transform.rotation;
            }

            progressQuestUponCombatVictory = quests;

            Debug.Log("Interacted - Load Combat from Arena/Dialogue with quests");
            loading = false;
            LoadingScene.instance.LoadScene(combatScene, lastScene, false);
        }

        public void LoadTutorial(GameObject player, E_Scenes lastScene, List<CharacterStats> newEnemies, List<Quest> quests)
        {
            if (loading || newEnemies.Count == 0) return;
            loading = true;

            //Get enemies within radius of player and save them in a list
            enemies.Clear();
            enemies = newEnemies;
            enemyIDs.Clear();

            //Saving last scene
            if (lastScene != E_Scenes.Null && player != null)
            {
                lastPos = player.transform.position;
                lastRot = player.transform.rotation;
            }

            progressQuestUponCombatVictory = quests;

            Debug.Log("Interacted - Load Combat from Arena/Dialogue with quests");
            loading = false;
            LoadingScene.instance.LoadScene(tutorialScene, lastScene, false);
        }

        public void AddEnemy(CharacterStats enemy, Vector2[] points, Object projectileObject, float projectileSpeed, Object impactObject, Object projectileFXObject, Color trailColor)
        {
            List<Vector2> targetPositions = new List<Vector2>();
            //targetPositions.Add(midPos);
            targetPositions.Add(queue.transform.position);

            VFXManager.instance.SpawnProjectile(points, projectileObject, projectileSpeed, trailColor, impactObject, projectileFXObject, E_DamageTypes.Physical);
            enemies.Add(enemy);
            queue.UpdateUI();
        }

        public void EnemiesDefeated()
        {
            foreach (string ID in enemyIDs)
            {
                interacted.Add(ID);
            }

            enemyIDs.Clear();

            if (progressQuestUponCombatVictory.Count > 0)
            {
                foreach (var item in progressQuestUponCombatVictory)
                    item.QuestProgress();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, combatRadius);
        }

        #region Quest Data

        public List<Quest> progressQuestUponCombatVictory;

        #endregion
    }

    [System.Serializable]
    public struct EnemySpawn
    {
        public CharacterStats stats;
        public Character spawner;
    }
}
