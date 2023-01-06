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
    public class BestiaryPage : MonoBehaviour
    {
        public Bestiary_Entry Page;

        public TMPro.TMP_Text EnemName;
        public Image EnemImg;
        public TMPro.TMP_Text EnemDesc;

        public TMPro.TMP_Text EnemLoca;
        public TMPro.TMP_Text EnemRes;
        public TMPro.TMP_Text EnemWeak;

        void Start()
        {
            EnemName.text = Page.Name;
            EnemImg.sprite = Page.Artwork;
            EnemDesc.text = Page.Description;
            EnemLoca.text = Page.Location;
            EnemRes.text = Page.Resistances;
            EnemWeak.text = Page.Weaknesses;
        }

        // Update is called once per frame
        void Update()
        {
            /*
            if(KILLCOUNT > T1req)
            {
                EnemName.text = Page.Name;
                EnemImg.Sprite = Page.Artwork;
                EnemDesc.text = Page.Description;
            } 

            if(KILLCOUNT > T2req)
            {
                EnemLoca.text = Page.Location;
            }

            if(KILLCOUNT > T3req)
            {
                EnemRes.text = Page.Resistances;
                EnemWeak.text = Page.Weaknesses;
            }
            */
        }
    }
}
