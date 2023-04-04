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
    public class TutorialMessageManager : MonoBehaviour
    {
        public static TutorialMessageManager instance;

        private void Start()
        {
            instance = this;
        }

        int turn = 0;
        int step = 0;

        public TutorialMessages[] turnMessages;

        public void ShowMessageTurn(int turn)
        {
            if (turn < turnMessages.Length)
            {
                //Show tooptip popup
                this.turn = turn;
                step = 0;
                ShowMessage();
            }
            else
            {
                TooltipManager.instance.ShowTutorialTooltip(false, "Tutorial", "");
            }
        }

        public void ProgressStep(bool advance)
        {
            step = advance ? step + 1 : step - 1;
            step = Mathf.Clamp(step, 0, turnMessages[turn].stepMessages.Length);
            ShowMessage();
        }

        void ShowMessage()
        {
            if (step < turnMessages[turn].stepMessages.Length)
            {
                //Set tooltip box to
                string message = turnMessages[turn].stepMessages[step].ToString();
                TooltipManager.instance.ShowTutorialTooltip(true, "Tutorial", message);
            }
            else
            {
                //Remove box/ keep last one
            }
        }
    }

    [System.Serializable]
    public struct TutorialMessages
    {
        public string[] stepMessages;
    }
}
