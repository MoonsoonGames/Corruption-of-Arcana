using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public RectTransform boxTransform;
        public Vector3 defaultPosition;
        public Vector3 defaultScale;

        private void Start()
        {
            instance = this;
            DragManager.instance.StartDragging += StartDragging;
            DragManager.instance.StopDragging += StopDragging;
        }

        int turn = 0;
        int step = 0;

        public TutorialMessages[] turnMessages;

        #region Showing Messages

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
                string message = turnMessages[turn].stepMessages[step].message.ToString();
                TooltipManager.instance.ShowTutorialTooltip(true, "Tutorial", message);

                boxTransform.position = turnMessages[turn].stepMessages[step].position;
                boxTransform.localScale = turnMessages[turn].stepMessages[step].scale;
            }
            else
            {
                //TooltipManager.instance.ShowTutorialTooltip(false, "Tutorial", "");
                //Remove box/ keep last one
                boxTransform.position = defaultPosition;
                boxTransform.localScale = defaultScale;
            }
        }

        public void EndTurn()
        {
            TooltipManager.instance.ShowTutorialTooltip(false, "Tutorial", "");

            //Remove box/ keep last one
            boxTransform.position = defaultPosition;
            boxTransform.localScale = defaultScale;
        }

        #endregion

        #region Dragging Cards

        public void StartDragging(CardDrag2D cardDrag)
        {
            if (cardDrag == null)
                return;

            if (turn >= turnMessages.Length || step >= turnMessages[turn].stepMessages.Length)
                return;

            Spell advanceOnPickup = turnMessages[turn].stepMessages[step].advanceOnPickup;

            if (advanceOnPickup == null)
                return;

            Card card = cardDrag.GetComponent<Card>();

            if (card.spell == advanceOnPickup)
            {
                Debug.Log("Tutorial checks true for " + cardDrag.name);
                ProgressStep(true);
            }
            else
            {
                Debug.Log("Tutorial checks false for " + cardDrag.name);
            }
        }

        public void StopDragging(CardDrag2D cardDrag, Character target)
        {
            if (target == null)
                return;

            if (turn >= turnMessages.Length || step >= turnMessages[turn].stepMessages.Length)
                return;

            bool advanceOnTarget = turnMessages[turn].stepMessages[step].advanceOnTarget;

            if (!advanceOnTarget)
                return;

            int targetCount = turnMessages[turn].stepMessages[step].target;

            Character check = targetCount < 0 ? CombatManager.instance.player : CombatManager.instance.enemyTeamManager.team[targetCount];

            if (check == null)
                return;

            if (target == check)
            {
                Debug.Log("Tutorial checks true for " + target.stats.characterName);
                ProgressStep(true);
            }
            else
            {
                Debug.Log("Tutorial checks false for " + target.stats.characterName);
            }
        }

        #endregion
    }

    [System.Serializable]
    public struct TutorialMessages
    {
        public TutorialMessagesTransform[] stepMessages;
    }

    [System.Serializable]
    public struct TutorialMessagesTransform
    {
        public string message;
        public Vector3 position;
        public Vector3 scale;

        public Spell advanceOnPickup;
        public bool advanceOnTarget;
        public int target;
    }
}
