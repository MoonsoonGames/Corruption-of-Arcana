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
        }

        public void StartDragging(CardDrag2D drag)
        {
            draggedCard = drag;

            if (TooltipManager.instance == null) return;

            TooltipManager.instance.EnableTooltips(draggedCard == null);
        }

        public bool canDrag;
    }
}