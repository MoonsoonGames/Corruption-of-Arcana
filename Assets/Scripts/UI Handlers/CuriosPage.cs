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

        public void SpiceCrystalBTN()
        {
            Curios_Object Item = (Curios_Object)Trinkets[0];
            PageName.text = Item.Name;
            PageImg.sprite = Item.Artwork;
            PageDesc.text = Item.Description;
        }

        public void ForbiddenRingBTN()
        {
            Curios_Object Item = (Curios_Object)Trinkets[1];
            PageName.text = Item.Name;
            PageImg.sprite = Item.Artwork;
            PageDesc.text = Item.Description;
        }

        void Update()
        {
            
        }
    }
}