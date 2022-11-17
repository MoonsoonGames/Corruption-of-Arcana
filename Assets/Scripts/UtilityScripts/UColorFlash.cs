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
    public class UColorFlash : MonoBehaviour
    {
        Image image;
        public float revertTime = 0.0005f;

        Color flashColour;
        float p = 0;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        public void Flash(E_DamageTypes effectType)
        {
            Debug.Log("Flash color");
            CancelInvoke();
            p = 0;

            flashColour = ColourFromDamageType(effectType);
            image.color = flashColour;

            InvokeRepeating("RevertColour", 0f, 0.05f);
        }

        void RevertColour()
        {
            image.color = LerpColour(p);

            p += revertTime;

            if (p == 1)
            {
                CancelInvoke();
                p = 0;
            }
        }

        Color LerpColour(float i)
        {
            Color lerpColour = new Color(0, 0, 0, 0);

            lerpColour.r = Mathf.Lerp(flashColour.r, normalColour.r, i);
            lerpColour.g = Mathf.Lerp(flashColour.g, normalColour.g, i);
            lerpColour.b = Mathf.Lerp(flashColour.b, normalColour.b, i);
            lerpColour.a = Mathf.Lerp(flashColour.a, normalColour.a, i);

            return lerpColour;
        }

        #region Colour

        public Color normalColour = new Color(255, 255, 255, 255);

        public Color physicalColour;
        public Color perforationColour;
        public Color septicColour;
        public Color bleakColour;
        public Color staticColour;
        public Color emberColour;

        public Color healColour;
        public Color shieldColour;
        public Color arcanaColour;

        public Color defaultColour;

        Color ColourFromDamageType(E_DamageTypes damageType)
        {
            switch (damageType)
            {
                case E_DamageTypes.Physical:
                    return physicalColour;
                case E_DamageTypes.Perforation:
                    return perforationColour;
                case E_DamageTypes.Septic:
                    return septicColour;
                case E_DamageTypes.Bleak:
                    return bleakColour;
                case E_DamageTypes.Static:
                    return staticColour;
                case E_DamageTypes.Ember:
                    return emberColour;

                case E_DamageTypes.Healing:
                    return healColour;
                case E_DamageTypes.Shield:
                    return shieldColour;
                case E_DamageTypes.Arcana:
                    return arcanaColour;
                default:
                    return defaultColour;
            }
        }

        #endregion
    }
}