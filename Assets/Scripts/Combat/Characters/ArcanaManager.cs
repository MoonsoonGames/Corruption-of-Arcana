using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class ArcanaManager : MonoBehaviour
    {
        public int arcanaMaxBase = 3;
        int arcanaMax = 3; public int GetMaxArcana() { return arcanaMax; }

        //public GameObject message;
        public GameObject buttonOverlay;
        public TextMeshProUGUI arcanaText;
        public Image arcanaImage;
        public Color enableColor;
        public Color disableColor;

        public bool silenced = false;

        public void AdjustArcanaMax(int change)
        {
            arcanaMax += change;

            //arcanaText.text = arcanaMax - arcanaMax + "/" + arcanaMax;
            //arcanaImage.color = enableColor;
        }

        public void CheckArcana(int arcana)
        {
            arcanaText.text = arcanaMax - arcana + "/" + arcanaMax;

            if (arcana <= arcanaMax)
            {
                //Debug.Log("Can cast");
                //Can cast, disable message and enable end turn
                buttonOverlay.SetActive(false);
                arcanaImage.color = enableColor;
            }
            else
            {
                //Debug.Log("Can't cast");
                //Can't cast, enable message and disable end turn
                buttonOverlay.SetActive(true);
                arcanaImage.color = disableColor;
            }
        }
    }
}