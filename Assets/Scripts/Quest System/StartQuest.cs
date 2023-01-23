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
    public class StartQuest : MonoBehaviour
    {
        public Quest quest;
        public string NPC;

        [ContextMenu("Start Quest")]
        public void BeginQuest()
        {
            quest.StartQuest(NPC, null);
        }
    }
}
