using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class DragManager : MonoBehaviour
    {
        #region Singleton
        //Code from last year

        public static DragManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                //DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Debug.Log("destroying " + gameObject.name + ", singleton: " + instance.gameObject.name);
                Destroy(gameObject);
            }
        }

        #endregion

        public Canvas canvas;
        public Camera UICam;
        public CardDrag2D draggedCard;

        private void Awake()
        {
            Singleton();
            StartDragging = StartDraggingLogic;
            StopDragging = StopDraggingLogic;
        }

        public delegate void DragDelegate(CardDrag2D card);
        public DragDelegate StartDragging;

        public void StartDraggingLogic(CardDrag2D drag)
        {
            draggedCard = drag;

            if (TooltipManager.instance == null) return;

            TooltipManager.instance.EnableTooltips(draggedCard == null);
        }

        public delegate void DragTargetDelegate(CardDrag2D card, Character target);
        public DragTargetDelegate StopDragging;

        public void StopDraggingLogic(CardDrag2D drag, Character target)
        {
            canDrag = true;
            StartDragging(null);
        }

        public bool canDrag;
    }
}