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
    [System.Serializable]
    public class QuestData
    {
        public Dictionary<string, int> questDict;
        public string fileName;

        public QuestData(List<Quest> questsToSave)
        {
            fileName = questsToSave[0].questName;
            questDict = new Dictionary<string, int>();
            foreach (Quest quest in questsToSave)
                questDict.Add(quest.name, quest.currentProgress);
        }
    }
}
