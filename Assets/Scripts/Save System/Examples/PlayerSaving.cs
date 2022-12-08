using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem.Serializables;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    /// <summary>
    /// Saves player location, health and other relevant stats
    /// 
    /// Extended by ISaveable
    /// </summary>
    public class PlayerSaving : MonoBehaviour, ISaveable
    {
        [SerializeField] private SerializableVector3 position;
        [SerializeField] private float health;
        [SerializeField] private int maxHealth;

        // Quest Saving vars
        [SerializeField] private int questStage = 0;

        // Enemy stat stuff
        [SerializeField] private int numberOfEnemiesDefeated = 0;


        public object CaptureState()
        {
            return new SaveData
            {
                position = position,
                health = health,
                maxHealth = maxHealth,
                questStage = questStage,
                numberOfEnemiesDefeated = numberOfEnemiesDefeated
            };
        }


        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            position = saveData.position;
            health = saveData.health;
            maxHealth = saveData.maxHealth;
            questStage = saveData.questStage;
            numberOfEnemiesDefeated = saveData.numberOfEnemiesDefeated;
        }

        [System.Serializable]
        private struct SaveData
        {
            public SerializableVector3 position;
            public float health;
            public int maxHealth;

            public int questStage;
            public int numberOfEnemiesDefeated;
        }
    }
}
