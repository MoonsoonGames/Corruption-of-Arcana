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
    public class DeckTab : MonoBehaviour
    {
        public GameObject hand, potions;
        public Image handImage, potionsImage;
        public Color baseColor = new Color (255, 255, 255, 255), selectedColor = new Color(100, 100, 100, 255);
        Vector3 basePos, offsetPos;

        private void Start()
        {
            basePos = hand.transform.position;
            offsetPos = basePos;
            offsetPos.y = -5000000;
            SelectHand();
        }

        public void SelectHand()
        {
            hand.transform.position = basePos;
            potions.transform.position = offsetPos;
            handImage.color = selectedColor;
            potionsImage.color = baseColor;
        }

        public void SelectPotions()
        {
            hand.transform.position = offsetPos;
            potions.transform.position = basePos;
            handImage.color = baseColor;
            potionsImage.color = selectedColor;
        }
    }
}
