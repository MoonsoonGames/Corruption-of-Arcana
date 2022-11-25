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
    public class BuildDeck : MonoBehaviour
    {
        List<Spell> spells;

        // Start is called before the first frame update
        void Start()
        {
            spells = new List<Spell>();
        }

        public void ClearSpells()
        {
            spells.Clear();
            DeckManager.instance.SetDeck(spells);
        }

        public void AddRemoveSpell(Spell spell, bool add)
        {
            if (add)
            {
                spells.Add(spell);
            }
            else
            {
                spells.Remove(spell);
            }

            DeckManager.instance.SetDeck(spells);
        }
    }
}
