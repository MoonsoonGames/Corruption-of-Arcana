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
            restartStatic = new Quest[restart.Length];

            for (int i = 0; i < restart.Length; i++)
            {
                restart[i].ForceRestartQuest();
                restartStatic[i] = restart[i];
            }

            resetStatic = new Quest[reset.Length];

            for (int i = 0; i < reset.Length; i++)
            {
                reset[i].ForceRestartQuest();
                resetStatic[i] = reset[i];
            }
        }

        public static Quest[] restartStatic;
        public static Quest[] resetStatic;

        public static void QuestResetStatic()
        {
            foreach (Quest r in restartStatic)
                r.ForceRestartQuest();
            foreach (Quest r in resetStatic)
                r.ForceResetQuest();
        }
    }
}
