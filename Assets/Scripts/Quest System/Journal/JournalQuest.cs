using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class JournalQuest : MonoBehaviour
    {
        QuestMenuUpdater updater;
        Quest quest;

        public Sprite[] stateImagesMain;
        public Sprite[] stateImagesSide;

        public TextMeshProUGUI nameText;
        public Image progressImage;

        public void Setup(QuestMenuUpdater newUpdater, Quest newQuest)
        {
            updater = newUpdater;
            quest = newQuest;

            nameText.text = newQuest.questName;
            progressImage.sprite = quest.mainQuest ? stateImagesMain[(int)quest.state] : stateImagesSide[(int)quest.state];
        }

        public void SelectQuest()
        {
            updater.SelectQuest(quest);
        }
    }
}
