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

        public Camera mainCam;

        public E_Scenes combatScene;
        public E_Scenes lastScene;
        public Vector3 lastPos;
        public Quaternion lastRot;

        public List<string> interacted;

        public float combatRadius = 15f;

        public EnemyQueue queue;
        public List<CharacterStats> enemies;
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
                    if (enemy.boss)
                    {
                        //If enemy is a boss, save them in the first space
                        enemies.Insert(0, enemy.enemyStats);
                    }
                    else
                    {
                        //Else append them at the end of the list
                        enemies.Insert(enemies.Count, enemy.enemyStats);
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

            foreach(var item in quests)
            {
                Debug.Log(item.questName);
            }

            progressQuestUponCombatVictory = quests;

            //Saving last scene
            if (lastScene != E_Scenes.Null)
            {
                lastPos = player.transform.position;
                lastRot = player.transform.rotation;
            }

            Debug.Log("Interacted - Load Combat");
            loading = false;
            LoadingScene.instance.LoadScene(combatScene, lastScene, false);
        }

        public void LoadCombat(List<CharacterStats> newEnemies, E_Scenes lastScene)
        {
            if (loading) return;
            loading = true;

            //Get enemies within radius of player and save them in a list
            enemies.Clear();
            enemies = newEnemies;
            enemyIDs.Clear();

            Debug.Log("Interacted - Load Combat from Arena");
            loading = false;
            LoadingScene.instance.LoadScene(combatScene, lastScene, false);
        }

        public void AddEnemy(CharacterStats enemy, Vector2[] points, Object projectileObject, float projectileSpeed, Object impactObject, Color trailColor)
        {
            List<Vector2> targetPositions = new List<Vector2>();
            //targetPositions.Add(midPos);
            targetPositions.Add(queue.transform.position);

            VFXManager.instance.SpawnProjectile(points, projectileObject, projectileSpeed, trailColor, impactObject, E_DamageTypes.Physical);
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
}
