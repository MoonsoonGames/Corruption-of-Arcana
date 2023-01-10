using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.SaveSystem;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SaveLoadCollection : MonoBehaviour, ISaveable
    {
        SpellReferenceTable spellRefTable;

        // Start is called before the first frame update
        void Start()
        {
            spellRefTable = GetComponent<SpellReferenceTable>();
        }

        public void SaveCards(List<Spell> collectedSpells, List<Spell> equippedSpells)
        {
            if (spellRefTable == null) { return; }

            foreach (var item in collectedSpells)
            {
                string reference = spellRefTable.GetReferenceDataFromSpell(item);

                //Save spells in collection
            }

            foreach (var item in equippedSpells)
            {
                string reference = spellRefTable.GetReferenceDataFromSpell(item);

                //Save equipped spells
            }
        }

        public void LoadCards(List<Spell> collectedSpells, List<Spell> equippedSpells)
        {
            if (spellRefTable == null) { return; }

            foreach (var item in collectedSpells)
            {
                string reference = spellRefTable.GetReferenceDataFromSpell(item);

                //Load spells in collection and add to list
            }

            foreach (var item in equippedSpells)
            {
                string reference = spellRefTable.GetReferenceDataFromSpell(item);

                //Load equipped spells and add to list
            }
        }

        List<string> collectedSpells;
        List<string> equippedSpells;

        public object CaptureState()
        {
            return new SaveData 
            { 
                collectedSpells = this.collectedSpells,
                equippedSpells = this.equippedSpells
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            collectedSpells = saveData.collectedSpells;
            equippedSpells = saveData.equippedSpells;
        }

        [System.Serializable]
        private struct SaveData
        {
            public List<string> collectedSpells;
            public List<string> equippedSpells;
        }
    }
}
