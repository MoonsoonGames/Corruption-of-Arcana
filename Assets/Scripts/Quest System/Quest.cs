using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem;

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

            //SaveQuestData();
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

        #region Saving and Loading

        [ContextMenu("Save Data")]
        public void SaveQuestData()
        {
            QuestSaving.SaveQuestData(RSaveQuestData());
        }

        [ContextMenu("Save Base Data")]
        public void SaveBaseQuestData()
        {
            QuestSaving.SaveBaseQuestData(RSaveQuestData());
        }


        List<Quest> RSaveQuestData()
        {
            List<Quest> state = new List<Quest>();

            state.Add(this);

            foreach (var item in subQuests)
            {
                List<Quest> newStates = item.RSaveQuestData();

                foreach (var stateItem in newStates)
                    state.Add(stateItem);
            }

            return state;
        }

        [ContextMenu("Load Data")]
        public void LoadQuestData()
        {
            QuestData questData = QuestSaving.LoadQuestData("/" + questName + "_quest.dat");

            if (questData == null) return;

            RLoadQuestData(questData);

            foreach (var item in subQuests)
            {
                item.RLoadQuestData(questData);
            }
        }

        [ContextMenu("Load Base Data")]
        public void LoadBaseQuestData()
        {
            QuestData questData = QuestSaving.LoadQuestData("/" + questName + "_questBase.dat");

            if (questData == null) return;

            RLoadQuestData(questData);

            foreach (var item in subQuests)
            {
                item.RLoadQuestData(questData);
            }
        }

        void RLoadQuestData(QuestData questData)
        {
            foreach (var item in questData.questDict)
            {
                if (item.Key == this.name)
                {
                    currentProgress = item.Value;

                    CheckProgress();
                }
            }
        }

        void CheckProgress()
        {
            if (currentProgress == -1)
                state = E_QuestStates.NotStarted;
            else if (currentProgress == maxProgress)
                state = E_QuestStates.Completed;
            else
                state = E_QuestStates.InProgress;
        }

        #endregion
    }
}
