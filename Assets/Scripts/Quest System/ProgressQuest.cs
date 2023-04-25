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
    public class ProgressQuest : MonoBehaviour
    {
        public void StartQuest(Quest quest, string npc)
        {
            quest.StartQuest(npc, null);
        }

        public void Progress(Quest quest)
        {
            quest.QuestProgress();
        }
    }
}
