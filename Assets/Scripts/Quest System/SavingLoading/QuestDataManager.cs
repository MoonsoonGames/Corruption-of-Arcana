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
        }

        public List<Quest> questsToSave;

        [ContextMenu("Save Quest Data")]
        public void SaveQuestData()
        {
            foreach (Quest quest in questsToSave)
            {
                quest.SaveQuestData();
            }
        }

        [ContextMenu("Save Base Quest Data")]
        public void SaveBaseQuestData()
        {
            foreach (Quest quest in questsToSave)
            {
                List<Quest> questList = new List<Quest> { quest };

                if (!QuestSaving.BaseDataExists(questList))
                    quest.SaveBaseQuestData();
            }
        }

        [ContextMenu("Load Quest Data")]
        public void LoadQuestData()
        {
            foreach (Quest quest in questsToSave)
            {
                quest.LoadQuestData();
            }
        }

        [ContextMenu("Load Base Quest Data")]
        public void LoadBaseQuestData()
        {
            foreach (Quest quest in questsToSave)
            {
                quest.LoadBaseQuestData();
            }
        }
    }
}
