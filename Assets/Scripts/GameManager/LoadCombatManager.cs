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
        }

        public E_Scenes combatScene;
        public float combatRadius = 15f;

        public EnemyQueue queue;
        public List<Object> enemies;

        public void LoadCombat(GameObject player)
        {
            //Get enemies within radius of player and save them in a list
            enemies.Clear();
            EnemyAI[] enemyAI = GameObject.FindObjectsOfType<EnemyAI>();

            foreach (EnemyAI enemy in enemyAI)
            {
                if (enemy.GetActive() && Vector3.Distance(player.transform.position, enemy.transform.position) < combatRadius)
                {
                    if (enemy.boss)
                    {
                        //If enemy is a boss, save them in the first space
                        enemies.Insert(0, enemy.enemyObject);
                    }
                    else
                    {
                        //Else append them at the end of the list
                        enemies.Insert(enemies.Count, enemy.enemyObject);
                    }
                }
            }

            Debug.Log("Interacted - Load Combat");
            LoadingScene.instance.LoadScene(combatScene);
        }

        public void AddEnemy(Object enemy, Vector3 spawnPos, Object projectileObject, Object impactObject, Color trailColor)
        {
            //Spawn effects here
            VFXManager.instance.SpawnProjectile(spawnPos, queue.transform.position, projectileObject, trailColor, impactObject, E_DamageTypes.Physical);
            enemies.Add(enemy);
            queue.UpdateUI();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, combatRadius);
        }
    }
}
