using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SimulateValues : MonoBehaviour
    {
        public TextMeshProUGUI minDmgText, maxDmgText, defaultDmgText, shieldText;
        public Color dmgColor, healColor;
        public GameObject deathIcon, healthObject, shieldObject;
        public EventReference deathSound;

        public void DisplayValues(Vector2Int dmg, int shd, bool willKill)
        {
            if (shieldObject != null)
                shieldObject.SetActive(shd > 0);
            shieldText.text = "+" + shd;

            if (dmg.x == dmg.y)
            {
                minDmgText.text = "";
                maxDmgText.text = "";

                #region Default Damage

                if (dmg.x < 0)
                {
                    healthObject.SetActive(true);
                    defaultDmgText.color = dmgColor;
                    defaultDmgText.text = dmg.x.ToString();
                }
                else if (dmg.x > 0)
                {
                    healthObject.SetActive(true);
                    defaultDmgText.color = healColor;
                    defaultDmgText.text = "+" + dmg.x;
                }
                else
                {
                    healthObject.SetActive(false);
                }

                #endregion
            }
            else
            {
                defaultDmgText.text = "";

                #region Min Damage

                if (dmg.x <= 0)
                {
                    healthObject.SetActive(true);
                    minDmgText.color = dmgColor;
                    minDmgText.text = dmg.x.ToString();
                }
                else if (dmg.x > 0)
                {
                    healthObject.SetActive(true);
                    minDmgText.color = healColor;
                    minDmgText.text = "+" + dmg.x;
                }

                #endregion

                #region Max Damage

                if (dmg.y <= 0)
                {
                    healthObject.SetActive(true);
                    maxDmgText.color = dmgColor;
                    maxDmgText.text = dmg.y.ToString();
                }
                else if (dmg.y > 0)
                {
                    healthObject.SetActive(true);
                    maxDmgText.color = healColor;
                    maxDmgText.text = "+" + dmg.y;
                }

                #endregion
            }


            //Debug.Log("will kill? " + willKill);

            deathIcon.SetActive(willKill);
            if (willKill)
            {
                //play sound here
                RuntimeManager.PlayOneShot(deathSound);
            }
        }
    }
}
