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

            SetTrackingQuest(baseQuest);
            UpdateQuestInfo();
        }

        public Quest baseQuest;
        Quest trackingQuest;

        public TextMeshProUGUI title, number, description;

        public void SetTrackingQuest(Quest quest)
        {
            trackingQuest = quest.GetParent();
            UpdateQuestInfo();
        }

        public void UpdateQuestInfo()
        {
            if (trackingQuest != null)
            {
                Quest sub = trackingQuest.GetCurrentQuestProgress();
                if (sub != null)
                {
                    title.text = sub.questName;
                    number.text = sub.currentProgress.ToString();
                    description.text = sub.questDescription;
                }
            }
        }
    }
}
