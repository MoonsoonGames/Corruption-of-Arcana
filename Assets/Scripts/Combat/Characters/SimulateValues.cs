using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SimulateValues : MonoBehaviour
    {
        public TextMeshProUGUI dmgText, shieldText;
        public Color dmgColor, healColor;
        public GameObject deathIcon, healthObject, shieldObject;


        int dmgRef, healRef, shfRef;
        int totalShield;
        int damageThroughShield;
        int totalDmg;

        public void DisplayValues(int dmg, int heal, int shd, bool willKill)
        {
            dmgRef = dmg;
            healRef = heal;
            shfRef = shd;
            totalShield = shd - dmg;
            damageThroughShield = 0;

            if (totalShield < 0)
                damageThroughShield = Mathf.Abs(totalShield);

            shieldObject.SetActive(totalShield > 0);
            shieldText.text = "+" + totalShield;

            totalDmg = heal - damageThroughShield;

            if (totalDmg < 0)
            {
                healthObject.SetActive(true);
                dmgText.color = dmgColor;
                dmgText.text = totalDmg.ToString();
            }
            else if (totalDmg > 0)
            {
                healthObject.SetActive(true);
                dmgText.color = healColor;
                dmgText.text = "+" + totalDmg;
            }
            else
            {
                healthObject.SetActive(false);
            }

            //Debug.Log("will kill? " + willKill);

            deathIcon.SetActive(willKill);
        }
    }
}
