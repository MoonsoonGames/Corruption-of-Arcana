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
    public class QuestQuickReset : MonoBehaviour
    {
        public Quest[] restart;
        public Quest[] reset;

        [ContextMenu("Quick Reset Quests")]
        public void QuestReset()
        {
            foreach (Quest r in restart)
                r.ForceRestartQuest();
            foreach (Quest r in reset)
                r.ForceResetQuest();
        }
    }
}
