using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem.Serializables;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.SaveSystem
{
    /// <summary>
    /// Saves player location, health and other relevant stats
    /// 
    /// Extended by ISaveable
    /// </summary>
    public class PlayerSaving : MonoBehaviour, ISaveable
    {
        // Player
        [Header("Player")]
        [SerializeField] private SerializableVector3 position;
        [SerializeField] private float health;
        [SerializeField] private int maxHealth;
        [SerializeField] private int gold;
        [SerializeField] private int arcana;
        [SerializeField] private List<UnityEngine.Object> curios;

        // Potions
        [Header("Potions")]
        [SerializeField] private int healthPotAmount;
        [SerializeField] private int ragePotAmount;
        [SerializeField] private int swiftPotAmount;
        [SerializeField] private int arcanaPotAmount;

        // Quest Saving vars // Enemy stat stuff
        [Header("Quest and Enemies")]
        [SerializeField] private int questStage = 0;
        [SerializeField] private int numberOfEnemiesDefeated = 0;




        /// <summary>
        /// Implemented class. Called when SavingLoading SAVES to disk.
        /// </summary>
        /// <returns></returns>
        public object CaptureState()
        {
            return new SaveData
            {
                position = position,
                health = health,
                maxHealth = maxHealth,
                gold = gold,
                arcana = arcana,
                healthPotAmount = healthPotAmount,
                ragePotAmount = ragePotAmount,
                swiftPotAmount = swiftPotAmount,
                arcanaPotAmount = arcanaPotAmount,
                curios = curios,
                questStage = questStage,
                numberOfEnemiesDefeated = numberOfEnemiesDefeated
            };
        }

        /// <summary>
        /// Implemented class. Called when SavingLoading LOADS from disk.
        /// </summary>
        /// <returns></returns>
        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            // Player
            position = saveData.position;
            health = saveData.health;
            maxHealth = saveData.maxHealth;
            gold = saveData.gold;
            arcana = saveData.arcana;
            // Potions
            healthPotAmount = saveData.healthPotAmount;
            ragePotAmount = saveData.ragePotAmount;
            swiftPotAmount = saveData.swiftPotAmount;
            arcanaPotAmount = saveData.arcanaPotAmount;
            // Inventory
            curios.AddRange(saveData.curios);
            // Quests and enemies
            questStage = saveData.questStage;
            numberOfEnemiesDefeated = saveData.numberOfEnemiesDefeated;
        }

        /// <summary>
        /// Savedata data structure
        /// </summary>
        [System.Serializable]
        private struct SaveData
        {
            public SerializableVector3 position;
            public float health;
            public int maxHealth;
            public int gold;
            public int arcana;

            public int healthPotAmount;
            public int ragePotAmount;
            public int swiftPotAmount;
            public int arcanaPotAmount;
            public List<UnityEngine.Object> curios;

            public int questStage;
            public int numberOfEnemiesDefeated;
        }
    }
}
