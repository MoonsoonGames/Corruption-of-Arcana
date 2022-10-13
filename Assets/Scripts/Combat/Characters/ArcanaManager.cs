using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class ArcanaManager : MonoBehaviour
    {
        public int arcanaMax = 3;

        //public GameObject message;
        public Button endTurnButton;
        public TextMeshProUGUI arcanaText;
        public Image arcanaImage;
        public Color enableColor;
        public Color disableColor;

        public void CheckArcana(int arcana)
        {
            if (arcana <= arcanaMax)
            {
                Debug.Log("Can cast");
                //Can cast, disable message and enable end turn
                endTurnButton.enabled = true;
                arcanaText.text = arcanaMax - arcana + "/" + arcanaMax;
                arcanaImage.color = enableColor;
            }
            else
            {
                Debug.Log("Can't cast");
                //Can't cast, enable message and disable end turn
                endTurnButton.enabled = false;
                arcanaText.text = arcanaMax - arcana + "/" + arcanaMax;
                arcanaImage.color = enableColor;
            }
        }
    }
}