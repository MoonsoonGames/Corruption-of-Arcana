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
    public class DebugValuesQuest : MonoBehaviour
    {
        public Quest quest;

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI stateText;

        void Update()
        {
            if (quest == null)
                return;

            if (nameText != null)
            {
                nameText.text = quest.name;
            }

            if (stateText != null)
            {
                stateText.text = quest.state.ToString();
            }
        }
    }
}
