using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class PotionManager : MonoBehaviour, ISaveable
    {
        #region Singleton
        //Code from last year

        public static PotionManager instance = null;

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

        public int healing = 2, haste = 1, recall = 2, poison = 1;

        public Dictionary<E_PotionType, int> potions = new Dictionary<E_PotionType, int>();

        private void Start()
        {
            Singleton();

            potions.Add(E_PotionType.Healing, healing);
            potions.Add(E_PotionType.Haste, haste);
            potions.Add(E_PotionType.Recall, recall);
            potions.Add(E_PotionType.Poison, poison);
        }

        public void ChangePotion(E_PotionType type, int addition)
        {
            potions[type] += addition;
            SaveManager.instance.SaveAllData();
        }

        public bool PotionAvailable(E_PotionType type, int cost)
        {
            Debug.Log("Potion of " + type.ToString() + " requires " + cost + " we have " + potions[type]);
            return (potions[type] >= cost);
        }

        public int GetPotionCount(E_PotionType type){
            return potions[type];
        }

        public object CaptureState()
        {
            return new SaveData
            {
                healing = this.healing,
                haste = this.haste,
                recall = this.recall,
                poison = this.poison
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            healing = saveData.healing;
            haste = saveData.haste;
            recall = saveData.recall;
            poison = saveData.poison;
        }

        public void ResetState()
        {
            healing = 0;
            haste = 0;
            recall = 0;
            poison = 0;
        }
        
        [System.Serializable]
        private struct SaveData
        {
            public int healing;
            public int haste;
            public int recall;
            public int poison;
        }
    }

    public enum E_PotionType
    {
        Healing, Haste, Recall, Poison
    }
}
