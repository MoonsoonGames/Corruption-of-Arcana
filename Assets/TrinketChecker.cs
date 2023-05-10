using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necropanda
{
    public class TrinketChecker : MonoBehaviour
    {
        public int TrinketNumber;
        CuriosPage curiosPage;
        TMPro.TMP_Text BtnText;

        void Start()
        {
            curiosPage = GameObject.FindObjectOfType<CuriosPage>();
        }
        void Update()
        {   
            BtnText = GetComponentInChildren<TMPro.TMP_Text>();
            Curios_Object Item = (Curios_Object)curiosPage.Trinkets[TrinketNumber];
            if(Item.isCollected == true)
            {
                BtnText.text = Item.Name;
            }
            else
            {
                BtnText.text = "???";
            }
        }
    }
}