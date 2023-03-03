using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class QuestDataManager : MonoBehaviour
    {
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
                Destroy(gameObject);
            }
        }

        #endregion

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
            SaveManager.instance.saveAllData += SaveQuestData;
            SaveManager.instance.saveAllBaseData += SaveBaseQuestData;
            SaveManager.instance.overideAllBaseData += OverideBaseQuestData;
            SaveManager.instance.loadAllData += LoadQuestData;
            SaveManager.instance.loadAllBaseData += LoadBaseQuestData;
        }

        public List<Quest> questsToSave;

        [ContextMenu("Save Quest Data")]
        public void SaveQuestData()
        {
            Debug.Log("Saving quest data");
            foreach (Quest quest in questsToSave)
            {
                quest.SaveQuestData();
            }
        }

        [ContextMenu("Save Base Quest Data")]
        public void SaveBaseQuestData()
        {
            Debug.Log("Saving base quest data");
            foreach (Quest quest in questsToSave)
            {
                List<Quest> questList = new List<Quest> { quest };

                if (!QuestSaving.BaseDataExists(questList))
                    quest.SaveBaseQuestData();
            }
        }

        [ContextMenu("Overide Base Quest Data")]
        public void OverideBaseQuestData()
        {
            Debug.Log("Saving base quest data");
            foreach (Quest quest in questsToSave)
            {
                quest.SaveBaseQuestData();
            }
        }

        [ContextMenu("Load Quest Data")]
        public void LoadQuestData()
        {
            Debug.Log("Loading quest data");
            foreach (Quest quest in questsToSave)
            {
                quest.LoadQuestData();
            }
        }

        [ContextMenu("Load Base Quest Data")]
        public void LoadBaseQuestData()
        {
            Debug.Log("Loading base quest data");
            foreach (Quest quest in questsToSave)
            {
                quest.LoadBaseQuestData();
            }
        }
    }
}
