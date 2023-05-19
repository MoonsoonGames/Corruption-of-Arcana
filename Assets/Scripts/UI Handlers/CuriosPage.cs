using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Necropanda;

/// <summary>
/// Authored & Written by <Jack Drage>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class CuriosPage : MonoBehaviour
    {
        public ScriptableObject[] Trinkets;
        public TMPro.TMP_Text PageName;
        public Image PageImg;
        public TMPro.TMP_Text PageDesc;
        public Sprite DefaultImg;

        public void SetText(Curios_Object item)
        {
            PageName.text = item.Name;
            PageImg.sprite = item.Artwork;
            PageDesc.text = item.Description;
        }

        public void DefultPage()
        {
            PageName.text = "???";
            PageImg.sprite = DefaultImg;
            PageDesc.text = "You have yet to collect this trinket".ToString();
        }

        public void TrinketBTN(int TrinketNumber)
        {
            Curios_Object Item = (Curios_Object)Trinkets[TrinketNumber];
            if(Item.isCollected == true)
            {
                SetText(Item);       
            }
            else 
            {
                DefultPage();
            }
        }
    }
}