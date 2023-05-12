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
        #region Setup

        #region Variables

        [Header("Basic Info")]
        public string questName;
        public bool mainQuest;
        public int questNumber;
        [TextArea(3, 10)]
        public string questDescription;
        string questGiver = "";

        [Header("Advanced Info")]
        public E_QuestStates state;
        public int currentProgress = -1;
        public int maxProgress = 1;

        public Quest parentQuest;
        public Quest[] subQuests;
        public bool linear = true;

        public LootPool rewards;
        public bool overrideParentRewards = false;

        #endregion

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

        public Quest GetParent()
        {
            if (parentQuest == null)
                return this;

            return parentQuest.GetParent();
        }

        #endregion

        #region Quest Progress

        public void StartQuest(string questGiver, Quest parent)
        {
            //Debug.Log("Start quest " + questName);

            if (parent != null)
                parentQuest = parent;

            this.questGiver = questGiver;
            state = E_QuestStates.InProgress;

            currentProgress = 0;

            EnableNextObjective();
            EnableAllObjectives();

            UpdateQuestInfo();
        }

        [ContextMenu("Quest Progress")]
        public void QuestProgress()
        {
            if (state != E_QuestStates.InProgress)
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

        void EnableAllObjectives()
        {
            if (!linear)
            {
                for (int i = 0; i < subQuests.Length; i++)
                {
                    subQuests[i].StartQuest(questGiver, this);
                }
            }

            UpdateQuestInfo();
        }

        void GiveRewards()
        {
            UpdateQuestInfo();

            if (overrideParentRewards)
            {
                OverrideRewards(rewards);
                return;
            }
            else
                rewards.RewardItems();
        }

        public void OverrideRewards(LootPool newRewards)
        {
            if (parentQuest == null)
            {
                rewards = newRewards;
            }
            else
            {
                parentQuest.OverrideRewards(newRewards);
            }
        }

        void UpdateQuestInfo()
        {
            if (QuestInfo.instance != null)
            {
                QuestInfo.instance.UpdateQuestInfo();
            }

            if (Application.isPlaying)
                GeneralDialogueLogic.instance.CheckQuestMarkers();
        }

        public Quest GetCurrentQuestProgress()
        {
            //currently this only allows quests with a linear progression, so no choices yet
            if (state != E_QuestStates.InProgress)
                return null;

            Quest quest = this;

            if (subQuests.Length > 0)
                quest = subQuests[currentProgress].GetCurrentQuestProgress();

            return quest;
        }

        #endregion

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
        }

        [ContextMenu("Load Base Data")]
        public void LoadBaseQuestData()
        {
            QuestData questData = QuestSaving.LoadQuestData("/" + questName + "_questBase.dat");

            if (questData == null)
            {
                Debug.LogError("Error with base quest data");
                return;
            }

            RLoadQuestData(questData);
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

            foreach (var item in subQuests)
            {
                item.RLoadQuestData(questData);
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

        #region Dev Command

        [ContextMenu("Force Quest - Cave Start")]
        public void TestQuestCommand13()
        {
            DevForceSetQuestProgress(13);
        }

        public void DevForceSetQuestProgress(int progress)
        {
            if (parentQuest != null)
            {
                parentQuest.DevForceSetQuestProgress(progress);
                return;
            }

            Debug.Log(questName + " is the parent for quest resetting");
            ForceRestartQuest();

            RForceSetQuestProgress(progress, 0);
        }

        public int RForceSetQuestProgress(int progress, int count)
        {
            if (subQuests.Length == 0)
            {
                while (count < progress && currentProgress < maxProgress)
                {
                    Debug.Log(questName + " is progressing " + count);
                    QuestProgress();
                    count++;
                }
                return count;  
            }

            foreach (var item in subQuests)
            {
                if (count < progress)
                {
                    Debug.Log(questName + " is progressing children (recursive) " + count);
                    count = item.RForceSetQuestProgress(progress, count);
                }
            }

            return count;
        }

        #endregion
    }
}
