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
    public class SpellReferenceTable : MonoBehaviour
    {
        public Spell[] saveableSpells;
        Dictionary<Spell, string> spellReferences;

        private void Start()
        {
            spellReferences = new Dictionary<Spell, string>();

            foreach (Spell spell in saveableSpells)
            {
                string spellRef = spell.spellName;

                spellReferences.Add(spell, spellRef);
            }
        }

        public string GetReferenceDataFromSpell(Spell spell)
        {
            string reference = "Null";

            foreach (var item in spellReferences)
            {
                if (item.Key == spell)
                {
                    reference = item.Value;
                }
            }

            return reference;
        }

        public Spell GetSpellFromReferenceData(string spellRef)
        {
            Spell reference = new Spell();

            foreach (var item in spellReferences)
            {
                if (item.Value == spellRef)
                {
                    reference = item.Key;
                }
            }

            return reference;
        }
    }
}