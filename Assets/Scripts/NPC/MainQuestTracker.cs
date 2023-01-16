using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class MainQuestTracker : MonoBehaviour
    {
        public GameObject[] state1, state2, state3, state4, state5, state6;

        public Flowchart flowchart;

        // Start is called before the first frame update
        void Start()
        {
            int questState = LoadCombatManager.instance.GetQuestState();

            Debug.Log("Quest state is " + questState);
            
            switch (questState)
            {
                case 1:
                    foreach (var item in state1)
                    {
                        item.SetActive(true);
                    }
                    break;
                case 2:
                    foreach (var item in state2)
                    {
                        item.SetActive(true);
                    }
                    break;
                case 3:
                    foreach (var item in state3)
                    {
                        item.SetActive(true);
                    }
                    break;
                case 4:
                    foreach (var item in state4)
                    {
                        item.SetActive(true);
                    }
                    break;
                case 6:
                    foreach (var item in state5)
                    {
                        item.SetActive(true);
                    }
                    break;
                case 7:
                    foreach (var item in state6)
                    {
                        item.SetActive(true);
                    }
                    break;
            }

            flowchart.SetIntegerVariable("QuestState", questState);
        }

        public void SetQuestState(int state)
        {
            LoadCombatManager.instance.UpdateQuestState(state);
        }
    }
}
