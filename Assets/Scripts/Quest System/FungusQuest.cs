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
    public class FungusQuest : MonoBehaviour
    {
        public Flowchart flowchart;

        public void SetFungusVariables(Quest quest)
        {
            if (flowchart != null)
                flowchart.SetIntegerVariable("QuestState", quest.currentProgress);
        }

        public void SetMouseActive(bool active)
        {
            if (active)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
