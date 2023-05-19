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

        public List<Spell> baseCollection;

        // Start is called before the first frame update
        void Start()
        {
            spellRefTable = GetComponent<SpellReferenceTable>();
            collectedSpellsSaved = new List<string>();
            equippedSpellsSaved = new List<string>();

            //LoadBaseCollection();
        }

        public void LoadBaseCollection()
        {
            SaveCards(baseCollection, new List<Spell>());
        }

        public void SaveCards(List<Spell> collectedSpells, List<Spell> equippedSpells)
        {
            return;

            if (spellRefTable == null) { return; }
            Debug.Log("Saving Cards");

            collectedSpellsSaved.Clear();
            equippedSpellsSaved.Clear();

            foreach (var item in collectedSpells)
            {
                string reference = spellRefTable.GetReferenceDataFromSpell(item);
                Debug.Log(reference + " is being saved");
                collectedSpellsSaved.Add(reference);
            }

            foreach (var item in equippedSpells)
            {
                string reference = spellRefTable.GetReferenceDataFromSpell(item);
                Debug.Log(reference + " is being saved");
                equippedSpellsSaved.Add(reference);
            }

            //GetComponentInChildren<SavingLoading>().Save();
        }

        public void LoadCards(List<Spell> collectedSpells, List<Spell> equippedSpells)
        {
            return;

            List<Spell> newCollection = new List<Spell>();
            List<Spell> newEquipped = new List<Spell>();

            //GetComponentInChildren<SavingLoading>().Load();

            if (spellRefTable == null) { return; }
            Debug.Log("Loading Cards");

            foreach (var item in collectedSpellsSaved)
            {
                Spell reference = spellRefTable.GetSpellFromReferenceData(item);
                Debug.Log(reference.spellName + " has been loaded");
                newCollection.Add(reference);
            }

            foreach (var item in equippedSpellsSaved)
            {
                Spell reference = spellRefTable.GetSpellFromReferenceData(item);
                Debug.Log(reference.spellName + " has been loaded");
                newEquipped.Add(reference);
            }

            collectedSpells = newCollection;
            equippedSpells = newEquipped;
        }

        List<string> collectedSpellsSaved;
        List<string> equippedSpellsSaved;

        public object CaptureState()
        {
            return new SaveData 
            { 
                collectedSpells = this.collectedSpellsSaved,
                equippedSpells = this.equippedSpellsSaved
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            collectedSpellsSaved = saveData.collectedSpells;
            equippedSpellsSaved = saveData.equippedSpells;
        }

        public List<Spell> defaultSpells;

        public void ResetState()
        {
            //TODO: Reset all values to default and then save them
            collectedSpellsSaved = new List<string>();
            equippedSpellsSaved = new List<string>();

            foreach (var item in defaultSpells)
            {
                equippedSpellsSaved.Add(spellRefTable.GetReferenceDataFromSpell(item));
            }

            CaptureState();
        }

        [System.Serializable]
        private struct SaveData
        {
            public List<string> collectedSpells;
            public List<string> equippedSpells;
        }
    }
}
