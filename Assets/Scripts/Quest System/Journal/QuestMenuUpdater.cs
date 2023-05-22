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
    public class QuestMenuUpdater : MonoBehaviour
    {
        public Quest[] quests;

        public VerticalLayoutGroup layoutGroup;
        public Object questPrefab;

        public void OpenJournal()
        {
            for (int i = 0; i < layoutGroup.transform.childCount; i++)
            {
                Destroy(layoutGroup.transform.GetChild(i).gameObject);
            }

            foreach (var item in quests)
            {
                if (item.state == E_QuestStates.InProgress || item.state == E_QuestStates.Completed)
                {
                    GameObject questObj = GameObject.Instantiate(questPrefab, layoutGroup.transform) as GameObject;

                    questObj.GetComponent<JournalQuest>().Setup(this, item);
                }
            }

            //Get current active quest
            //SelectQuest(activeQuest);
        }

        static Quest selectedQuest;
        public TextMeshProUGUI nameText, stateText, descriptionText, objName, objDescText;

        public void SelectQuest(Quest quest)
        {
            if (quest == null) return;
            //Debug.Log("Select quest " + quest.questName);
            selectedQuest = quest;

            nameText.text = selectedQuest.questName;
            stateText.text = selectedQuest.state.ToString();
            descriptionText.text = selectedQuest.questDescription;

            if (selectedQuest.state == E_QuestStates.Completed)
            {
                objName.text = "Completed";
                objDescText.text = "Quest is complete";
            }
            else
            {
                Quest objective = selectedQuest.GetCurrentQuestProgress();

                objName.text = objective.questName;
                objDescText.text = objective.questDescription;
            }

            ActivateQuest(); //TODO - Temp, add proper button for this later
        }

        public void ActivateQuest()
        {
            //Set active quest as current quest
            QuestInfo.instance.SetTrackingQuest(selectedQuest);
            QuestInfo.instance.UpdateQuestInfo();
        }

        public void Setup()
        {
            //Debug.Log("Select Quest (Start)");
            SelectQuest(selectedQuest);
        }
    }
}
