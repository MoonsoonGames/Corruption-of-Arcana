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
    public class QuestUnlockInteraction : RequireQuestProgress
    {
        public Interactable.Interactable interactable;

        protected override void SetMarker(bool active)
        {
            if (active)
                interactable.UnlockInteraction();
        }
    }
}
