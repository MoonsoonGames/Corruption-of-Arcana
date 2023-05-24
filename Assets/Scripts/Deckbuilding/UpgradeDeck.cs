using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class UpgradeDeck : BuildDeck
    {
        public override IEnumerator OpenMenu(float delay, float delay2)
        {
            yield return new WaitForSeconds(delay);

            if (collectedDeck.CurrentCardsLength() > 0)
            {
                collectedSpells = collectedDeck.GetSpells();
            }
            else
            {
                collectedSpells = new List<Spell>();
            }
        }

        public override void CloseMenu()
        {
            TEMP_OpenDeckbuilding.instance.OpenCloseMenu(false, this.gameObject);
        }

        public override bool SaveCards()
        {
            Debug.Log("Upgrade menu saves cards");
            DeckManager.instance.collection = collectedSpells;
            SaveManager.instance.SaveAllData();

            if (true /*Check that player has not left an upgraded card in the output*/)
            {
                //DeckManager.instance.collection = collectedSpells;

                //DeckManager.instance.SaveDeck();
            }

            return true;
        }
    }
}
