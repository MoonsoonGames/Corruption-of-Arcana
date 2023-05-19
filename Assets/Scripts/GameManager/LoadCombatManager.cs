using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.AI;
using Necropanda.SaveSystem;
using Necropanda.Utils.Console.Commands;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class LoadCombatManager : MonoBehaviour, ISaveable
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
        void Awake()
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

        [Space]

        // Player
        [Header("Player")]
        [SerializeField] private float posX, posY, posZ;
        [SerializeField] private float health;

        // Potions
        [Header("Potions")]
        // [SerializeField] private int healthPotAmount;
        // [SerializeField] private int ragePotAmount;
        // [SerializeField] private int swiftPotAmount;
        // [SerializeField] private int arcanaPotAmount;

        // Quest Saving vars // Enemy stat stuff
        [Header("Quest and Enemies")]


        [Header("Other Savables")]
        [SerializeField] private string sceneName;

        [SerializeField] private List<string> splitCollection = new List<string>();
        [SerializeField] private List<string> splitMajorArcana = new List<string>();
        // [SerializeField] private GiveCommand giveCommand;


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

            foreach (var item in quests)
            {
                Debug.Log(item.questName);
            }

            progressQuestUponCombatVictory = quests;

            //Saving last scene
            if (player != null)
            {
                lastPos = player.transform.position;
                lastRot = player.transform.rotation;
            }

            Debug.Log("Interacted - Load Combat");
            loading = false;
            LoadingScene.instance.LoadScene(combatScene, lastScene, 0);
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

                enemySpawnList.Add(enemySpawn);
            }

            enemies = enemySpawnList;
            enemyIDs.Clear();

            //Saving last scene
            if (player != null)
            {
                lastPos = player.transform.position;
                lastRot = player.transform.rotation;
            }

            progressQuestUponCombatVictory = quests;

            Debug.Log("Interacted - Load Combat from Arena/Dialogue with quests");
            loading = false;
            LoadingScene.instance.LoadScene(combatScene, lastScene, 0);
        }

        public void LoadTutorial(GameObject player, E_Scenes lastScene, List<CharacterStats> newEnemies, List<Quest> quests)
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

                enemySpawnList.Add(enemySpawn);
            }

            enemies = enemySpawnList;
            enemyIDs.Clear();

            //Saving last scene
            if (player != null)
            {
                lastPos = player.transform.position;
                lastRot = player.transform.rotation;
            }

            progressQuestUponCombatVictory = quests;

            Debug.Log("Interacted - Load Combat from Arena/Dialogue with quests");
            loading = false;
            LoadingScene.instance.LoadScene(tutorialScene, lastScene, 0);
        }

        public void AddEnemy(CharacterStats enemy, Character caster, Vector2[] points, UnityEngine.Object projectileObject, float projectileSpeed, UnityEngine.Object impactObject, UnityEngine.Object projectileFXObject, Color trailColor)
        {
            List<Vector2> targetPositions = new List<Vector2>();
            //targetPositions.Add(midPos);
            targetPositions.Add(queue.transform.position);

            VFXManager.instance.SpawnProjectile(points, projectileObject, projectileSpeed, trailColor, impactObject, projectileFXObject, E_DamageTypes.Physical);

            EnemySpawn enemySpawn = new EnemySpawn();
            enemySpawn.stats = enemy;
            enemySpawn.spawner = caster;

            enemies.Add(enemySpawn);
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
                    item.QuestProgress(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, combatRadius);
        }

        /// <summary>
        /// Implemented class. Called when SavingLoading SAVES to disk.
        /// </summary>
        /// <returns></returns>
        public object CaptureState()
        {
            //Debug.Log("Saving Player");


            return new SaveData
            {
                posX = lastPos.x,
                posY = lastPos.y,
                posZ = lastPos.z,

                rotX = lastRot.x,
                rotY = lastRot.y,
                rotZ = lastRot.z,
                rotW = lastRot.w,

                health = health,

                sceneName = LoadingScene.instance.loadScene.ToString(),

                interactedWith = interacted,

                savedCollection = string.Join(", ", (object[])DeckManager.instance.collection.ToArray()),
                savedMajorArcana = string.Join(", ", (object[])DeckManager.instance.majorArcana.ToArray()),

                // savedCollection = DeckManager.instance.collection,
                // savedMajorArcana = DeckManager.instance.majorArcana,
                // maxHealth = maxHealth,
                // gold = gold,
                // maxArcana = maxArcana,
                // healthPotAmount = healthPotAmount,
                // ragePotAmount = ragePotAmount,
                // swiftPotAmount = swiftPotAmount,
                // arcanaPotAmount = arcanaPotAmount,
                // curios = curios,
                // questStage = questStage,
                // numberOfEnemiesDefeated = numberOfEnemiesDefeated
            };
        }

        /// <summary>
        /// Implemented class. Called when SavingLoading LOADS from disk.
        /// </summary>
        /// <returns></returns>
        public void RestoreState(object state)
        {
            //Debug.Log("Restoring");
            var saveData = (SaveData)state;

            // Player
            Vector3 pos = new Vector3(saveData.posX, saveData.posY, saveData.posZ);
            Quaternion rot = new Quaternion(saveData.rotX, saveData.rotY, saveData.rotZ, saveData.rotW);
            lastPos = pos;
            health = saveData.health;

            E_Scenes scene = E_Scenes.Null;

            scene = HelperFunctions.StringToSceneEnum(saveData.sceneName);

            if (scene != E_Scenes.Null)
            {
                GameObject.FindObjectOfType<LoadingScene>().loadScene = scene;
            }

            // Clear any interactions for sanitary purposes, then load the save data into it.
            interacted = new List<string>(saveData.interactedWith);

            // // Use give command to add them to the inventory
            // splitCollection = ListifyString(saveData.savedCollection);
            // splitMajorArcana = ListifyString(saveData.savedMajorArcana);

            // DeckManager.instance.collection.Clear();
            // DeckManager.instance.majorArcana.Clear();

            // foreach (string card in splitCollection)
            // {
            //     Debug.Log(card);
            //     giveCommand.GiveToPlayer(card);
            // }

            // foreach (string card in splitMajorArcana)
            // {
            //     //Instead of this, add it to the 
            //     Debug.Log(card);

            //     if (giveCommand != null)
            //     {
            //         giveCommand.EquipToPlayer(card);
            //     }
            //     else
            //         Debug.Log("no give command");
            // }

            // maxHealth = saveData.maxHealth;
            // gold = saveData.gold;
            // maxArcana = saveData.maxArcana;

            // // Potions
            // healthPotAmount = saveData.healthPotAmount;
            // ragePotAmount = saveData.ragePotAmount;
            // swiftPotAmount = saveData.swiftPotAmount;
            // arcanaPotAmount = saveData.arcanaPotAmount;
            // // Inventory
            // curios.AddRange(saveData.curios);
        }

        public void ResetState()
        {
            //TODO: Reset all values to default and then save them
            lastPos = Vector3.negativeInfinity;

            GameObject.FindObjectOfType<LoadingScene>().loadScene = E_Scenes.Null;

            // Clear any interactions for sanitary purposes, then load the save data into it.
            interacted = new List<string>();

            CaptureState();
        }

        /// <summary>
        /// Savedata data structure
        /// </summary>
        [System.Serializable]
        private struct SaveData
        {
            public float posX, posY, posZ;
            public float rotX, rotY, rotZ, rotW;
            public float health;
            public string sceneName;

            public List<string> interactedWith;
            public string savedCollection;
            public string savedMajorArcana;
            // public int maxHealth;
            // public int gold;
            // public int maxArcana;

            // public int healthPotAmount;
            // public int ragePotAmount;
            // public int swiftPotAmount;
            // public int arcanaPotAmount;
            // public List<UnityEngine.Object> curios;
        }

        #region Quest Data

        public List<Quest> progressQuestUponCombatVictory;

        #endregion

        public List<string> ListifyString(string stringToProcess)
        {
            string thingToReplace = " (Necropanda.Spell)";
            string processedString = stringToProcess.Replace(thingToReplace, "");
            processedString = processedString.Replace(" ", string.Empty);
            string[] splitStrings = processedString.Split(',');

            List<string> newString = new List<string>();

            //Check to make sure each item is bigger than 1 character.
            foreach (var item in splitStrings)
            {
                if (item.Length > 1)
                    newString.Add(item);
            }

            return newString;
        }
    }



    [System.Serializable]
    public struct EnemySpawn
    {
        public CharacterStats stats;
        public Character spawner;
        public bool endCombatOnKill;
    }
}
