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
            if (spellRefTable == null) { return; }
            Debug.Log("Saving Cards");

            collectedSpellsSaved.Clear();
            equippedSpellsSaved.Clear();

            var spells = Resources.LoadAll("Spells", typeof(Spell));

            Spell[] allSpells = Resources.FindObjectsOfTypeAll<Spell>();
            
            foreach (var item in allSpells)
            {
                if (collectedSpells.Contains(item))
                {
                    Debug.Log(item.spellName + " should be saving in collection");
                    collectedSpellsSaved.Add(item.name);
                }

                if (equippedSpells.Contains(item))
                {
                    Debug.Log(item.spellName + " should be saving in equiped");
                    equippedSpellsSaved.Add(item.name);
                }
            }
        }

        public void LoadCards(List<Spell> collectedSpells, List<Spell> equippedSpells)
        {
            List<Spell> newCollection = new List<Spell>();
            List<Spell> newEquipped = new List<Spell>();

            //GetComponentInChildren<SavingLoading>().Load();

            if (spellRefTable == null) { return; }
            Debug.Log("Loading Cards");

            var spells = Resources.LoadAll("Spells", typeof(Spell));

            Spell[] allSpells = Resources.FindObjectsOfTypeAll<Spell>();
            
            foreach (var item in allSpells)
            {
                if (collectedSpellsSaved.Contains(item.name))
                {
                    Debug.Log(item.spellName + " should be loaded in collection");
                    newCollection.Add(item);
                }

                if (equippedSpellsSaved.Contains(item.name))
                {
                    Debug.Log(item.spellName + " should be loaded in equiped");
                    newEquipped.Add(item);
                }
            }

            DeckManager.instance.collection = new List<Spell>();
            DeckManager.instance.majorArcana = new List<Spell>();

            foreach(var item in newCollection)
            {
                Debug.Log("Trying to load: " + item.spellName);
                DeckManager.instance.collection.Add(item);
            }

            foreach(var item in newEquipped)
            {
                Debug.Log("Trying to load: " + item.spellName);
                DeckManager.instance.majorArcana.Add(item);
            }
        }

        List<string> collectedSpellsSaved;
        List<string> equippedSpellsSaved;

        public object CaptureState()
        {
            SaveCards(DeckManager.instance.collection, DeckManager.instance.majorArcana);

            return new SaveData 
            { 
                collectedSpells = this.collectedSpellsSaved,
                equippedSpells = this.equippedSpellsSaved
            };
        }

        public void RestoreState(object state)
        {
            Debug.Log("Loading Cards");
            var saveData = (SaveData)state;

            collectedSpellsSaved = saveData.collectedSpells;
            equippedSpellsSaved = saveData.equippedSpells;

            LoadCards(DeckManager.instance.collection, DeckManager.instance.majorArcana);
        }

        public List<Spell> defaultSpells;

        public void ResetState()
        {
            Debug.Log("Resetting Cards");
            //TODO: Reset all values to default and then save them
            collectedSpellsSaved = new List<string>();
            equippedSpellsSaved = new List<string>();

            DeckManager.instance.collection.Clear();
            DeckManager.instance.majorArcana.Clear();
            DeckManager.instance.unlockedWeapons.Clear();

            foreach (var item in defaultSpells)
            {
                DeckManager.instance.majorArcana.Add(item);
            }
        }

        [System.Serializable]
        private struct SaveData
        {
            public List<string> collectedSpells;
            public List<string> equippedSpells;
        }

        public List<string> ListifyString(string stringToProcess)
        {
            string thingToReplace = " (Necropanda.Spell)";
            string processedString = stringToProcess.Replace(thingToReplace, "");
            processedString = processedString.Replace(" ", string.Empty);
            string[] splitStrings = processedString.Split(',');

            List<string> cleanedList = new List<string>();

            foreach (string str in splitStrings)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    cleanedList.Add(str);
                }
                else
                {
                    // Log the empty string to the console
                    Debug.Log("Empty string found.");
                }
            }

            return cleanedList;
        }
    }
}
