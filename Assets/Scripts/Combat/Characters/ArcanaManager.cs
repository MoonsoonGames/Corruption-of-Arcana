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
        public SliderValue arcanaSlider;
        public Color enableColor;
        public Color disableColor;

        public bool silenced = false;

        private void Start()
        {
            arcanaSlider.Setup(arcanaMax);
            CheckCardOverlays(arcanaMax);
        }

        public void AdjustArcanaMax(int change)
        {
            arcanaMax += change;
            arcanaSlider.SetSliderMax(arcanaMax);
            CheckCardOverlays(arcanaMax);
            //arcanaText.text = arcanaMax - arcanaMax + "/" + arcanaMax;
            //arcanaImage.color = enableColor;
        }

        public void CheckArcana(int arcana)
        {
            arcanaSlider.SetSliderValue(arcana);
            //Debug.Log("Arcana is " + arcana + "/" + arcanaMax);
            if (arcana <= arcanaMax)
            {
                //Debug.Log("Can cast");
                //Can cast, disable message and enable end turn
                buttonOverlay.SetActive(false);
                arcanaSlider.standardFill.color = enableColor;
            }
            else
            {
                //Debug.Log("Can't cast");
                //Can't cast, enable message and disable end turn
                buttonOverlay.SetActive(true);
                arcanaSlider.standardFill.color = disableColor;
            }

            CheckCardOverlays(arcana);
        }

        void CheckCardOverlays(int arcana)
        {
            GameObject[] cardObjects = GameObject.FindGameObjectsWithTag("Card");
            //Debug.Log("Length is " + cardObjects.Length);
            foreach (var item in cardObjects)
            {
                Card itemCard = item.GetComponent<Card>();

                if (itemCard != null)
                    itemCard.ShowUnavailableOverlay(arcanaMax - arcana);
                else
                {
                    Debug.Log("Invalid");
                }
            }
        }
    }
}