using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class QuestInfo : MonoBehaviour
    {
        public static QuestInfo instance;

        private void Start()
        {
            instance = this;

            UpdateQuestInfo();
        }

        public Quest trackingQuest;

        public TextMeshProUGUI title, number, description;

        public void UpdateQuestInfo()
        {
            if (trackingQuest != null)
            {
                title.text = trackingQuest.questName;
                number.text = trackingQuest.currentProgress.ToString();
                description.text = trackingQuest.GetCurrentQuestProgress().questDescription;
            }
        }
    }
}
