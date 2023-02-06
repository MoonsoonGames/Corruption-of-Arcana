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
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        public string questName;
        public int questNumber;
        [TextArea(3, 10)]
        public string questDescription;
        string questGiver = "";

        public E_QuestStates state;
        public int currentProgress = -1;
        public int maxProgress = 1;

        public Quest parentQuest;
        public Quest[] subQuests;
        public bool linear = true;

        //public RewardsPool rewards
        //public int rewardNumber

        [ContextMenu("Force Restart Quest")]
        public void ForceRestartQuest()
        {
            ForceResetQuest();
            StartQuest("Mama R", null);
            UpdateQuestInfo();
        }

        [ContextMenu("Force Start Quest")]
        public void ForceStartQuest()
        {
            StartQuest("Mama R", null);
            UpdateQuestInfo();
        }

        [ContextMenu("Force Reset Quest")]
        public void ForceResetQuest()
        {
            state = E_QuestStates.NotStarted;
            currentProgress = -1;

            foreach (Quest quest in subQuests)
            {
                quest.ForceResetQuest();
            }

            UpdateQuestInfo();
        }

        public void StartQuest(string questGiver, Quest parent)
        {
            if (state != E_QuestStates.NotStarted)
                return;

            if (parent != null)
                parentQuest = parent;

            this.questGiver = questGiver;
            state = E_QuestStates.InProgress;

            currentProgress = 0;

            EnableNextObjective();

            UpdateQuestInfo();
        }

        [ContextMenu("Quest Progress")]
        public void QuestProgress()
        {
            //currently this only allows quests with a linear progression, so no choices yet
            if (state != E_QuestStates.InProgress && linear)
                return;

            currentProgress++;

            if (currentProgress >= maxProgress)
            {
                if (parentQuest != null)
                {
                    parentQuest.QuestProgress();
                }

                state = E_QuestStates.Completed;
                GiveRewards();
            }
            else
            {
                EnableNextObjective();
            }

            UpdateQuestInfo();
        }

        void EnableNextObjective()
        {
            if (subQuests.Length > 0)
            {
                if (linear)
                {
                    subQuests[currentProgress].StartQuest(questGiver, this);
                }
            }

            UpdateQuestInfo();
        }

        void GiveRewards()
        {
            UpdateQuestInfo();
            //could have this depend on how the quest was finished
            //rewards.GiveRewards
        }

        void UpdateQuestInfo()
        {
            if (QuestInfo.instance != null)
            {
                QuestInfo.instance.UpdateQuestInfo();
            }
        }

        public Quest GetCurrentQuestProgress()
        {
            //currently this only allows quests with a linear progression, so no choices yet
            if (state != E_QuestStates.InProgress)
                return null;

            Quest quest = null;

            if (subQuests.Length == 0)
            {
                if (state == E_QuestStates.InProgress)
                {
                    quest = this;
                }
            }
            else
            {
                quest = subQuests[currentProgress].GetCurrentQuestProgress();
            }

            return quest;
        }
    }
}
