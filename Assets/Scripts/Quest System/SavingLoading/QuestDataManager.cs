using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class QuestDataManager : MonoBehaviour, ISaveable
    {
        //C:\Users\as243879\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        //C:\Users\mr232432\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled

        #region Singleton
        //Code from last year

        public static QuestDataManager instance = null;

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
                //Destroy(gameObject);
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
        }

        public List<Quest> questsToSave;

        private Dictionary<string, int> SaveQuestData()
        {
            //Debug.Log("Saving quest data");

            Dictionary<string, int> allStates = new Dictionary<string, int>();

            foreach (var quest in questsToSave)
            {
                HelperFunctions.CombineDictionaries(allStates, quest.SaveQuestData());
            }

            return allStates;
        }

        private void LoadQuestData(Dictionary<string, int> allStates)
        {
            var quests = Resources.LoadAll("Quests", typeof(Quest));

            // Find the items
            Quest[] questList = Resources.FindObjectsOfTypeAll<Quest>();

            foreach (Quest quest in questList)
            {
                if (allStates.ContainsKey(quest.name))
                {
                    quest.LoadQuestData(allStates[quest.name]);
                }
            }
        }

        private void ResetQuestData()
        {
            QuestQuickReset.QuestResetStatic();
            //TODO: delete save data
        }

        public object CaptureState()
        {
            Dictionary<string, int> states = SaveQuestData();

            return new SaveData
            {
                #region QUEST SAVING
                quests = states
                #endregion
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            #region QUEST LOADING
            // QUEST LOADING
            LoadQuestData(saveData.quests);
            #endregion
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

            public Dictionary<string, int> quests;
        }

        public void ResetState()
        {
            ResetQuestData();
        }

        public Dictionary<Quest, int> ConvertStringToQuest(Dictionary<string, int> stringDict)
        {
            Dictionary<Quest, int> questDict = new Dictionary<Quest, int>();

            //Convert string dict to quest dict

            // Find all quests in the project


            return questDict;
        }
    }
}
