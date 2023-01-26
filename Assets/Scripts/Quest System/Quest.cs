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
        public int currentProgress = 0;
        public int maxProgress = 1;

        public Quest parentQuest;
        public Quest[] subQuests;
        public bool linear = true;

        //public RewardsPool rewards
        //public int rewardNumber

        [ContextMenu("Force Restart Quest")]
        public void ForceRestartQuest()
        {
            DebugQuest();
            ForceResetQuest();
            StartQuest("Mama R", null);
        }

        [ContextMenu("Force Start Quest")]
        public void ForceStartQuest()
        {
            DebugQuest();
            StartQuest("Mama R", null);
        }

        [ContextMenu("Force Reset Quest")]
        public void ForceResetQuest()
        {
            DebugQuest();
            state = E_QuestStates.NotStarted;
            currentProgress = 0;

            foreach (Quest quest in subQuests)
            {
                quest.ForceResetQuest();
            }
        }

        public void StartQuest(string questGiver, Quest parent)
        {
            DebugQuest();
            if (state != E_QuestStates.NotStarted)
                return;

            if (parent != null)
                parentQuest = parent;

            this.questGiver = questGiver;
            state = E_QuestStates.InProgress;

            EnableNextObjective();
        }

        [ContextMenu("Quest Progress")]
        public void QuestProgress()
        {
            DebugQuest();
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
        }

        void EnableNextObjective()
        {
            DebugQuest();
            if (subQuests.Length > 0)
            {
                if (linear)
                {
                    subQuests[currentProgress].StartQuest(questGiver, this);
                }
            }
        }

        void GiveRewards()
        {
            DebugQuest();
            //could have this depend on how the quest was finished
            //rewards.GiveRewards
        }

        void DebugQuest()
        {
            Debug.Log("Force reset quest " + questName);
        }
    }
}