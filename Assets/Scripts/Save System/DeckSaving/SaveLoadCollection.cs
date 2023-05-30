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
        public Weapon defaultWeapon;

        public List<Spell> defaultSpells;

        // Start is called before the first frame update
        void Start()
        {
            collectedSpellsSaved = new List<string>();
            equippedSpellsSaved = new List<string>();
            collectedWeaponsSaved = new List<string>();
            //LoadBaseCollection();
        }

        public void SaveCards(List<Spell> collectedSpells, List<Spell> equippedSpells)
        {
            collectedSpellsSaved.Clear();
            equippedSpellsSaved.Clear();

            var spells = Resources.LoadAll("Spells", typeof(Spell));

            Spell[] allSpells = Resources.FindObjectsOfTypeAll<Spell>();
            
            foreach(var item in collectedSpells)
            {
                foreach (var resource in allSpells)
                {
                    if (item == resource)
                        collectedSpellsSaved.Add(item.name);
                }
            }

            foreach (var item in equippedSpells)
            {
                foreach (var resource in allSpells)
                {
                    if (item == resource)
                        equippedSpellsSaved.Add(item.name);
                }
            }

            collectedWeaponsSaved.Clear();

            var weapons = Resources.LoadAll("Weapons", typeof(Weapon));

            Weapon[] allWeapons = Resources.FindObjectsOfTypeAll<Weapon>();

            foreach (var item in DeckManager.instance.unlockedWeapons)
            {
                foreach (var resource in allWeapons)
                {
                    if (item == resource)
                        collectedWeaponsSaved.Add(item.name);
                }
            }

            foreach (var resource in allSpells)
            {
                if (DeckManager.instance.weapon == resource)
                    equippedWeaponSaved = resource.name;
            }
        }

        public void LoadCards(List<Spell> collectedSpells, List<Spell> equippedSpells)
        {
            #region Spells

            List<Spell> newCollection = new List<Spell>();
            List<Spell> newEquipped = new List<Spell>();

            var spells = Resources.LoadAll("Spells", typeof(Spell));

            Spell[] allSpells = Resources.FindObjectsOfTypeAll<Spell>();

            foreach (var item in collectedSpellsSaved)
            {
                foreach (var resource in allSpells)
                {
                    if (item == resource.name)
                        newCollection.Add(resource);
                }
            }

            foreach (var item in equippedSpellsSaved)
            {
                foreach (var resource in allSpells)
                {
                    if (item == resource.name)
                        newEquipped.Add(resource);
                }
            }

            DeckManager.instance.collection = new List<Spell>();
            DeckManager.instance.majorArcana = new List<Spell>();

            foreach(var item in newCollection)
            {
                DeckManager.instance.collection.Add(item);
            }

            foreach(var item in newEquipped)
            {
                DeckManager.instance.majorArcana.Add(item);
            }

            #endregion

            #region Weapons

            List<Weapon> newWeapons = new List<Weapon>();

            var weapons = Resources.LoadAll("Weapons", typeof(Weapon));

            Weapon[] allWeapons = Resources.FindObjectsOfTypeAll<Weapon>();
            
            foreach (var item in allWeapons)
            {
                if (collectedWeaponsSaved.Contains(item.name))
                {
                    Debug.Log(item.weaponName + " should be loaded in collection");
                    newWeapons.Add(item);
                }

                if (equippedWeaponSaved == item.name)
                {
                    DeckManager.instance.weapon = item;
                }
            }

            DeckManager.instance.unlockedWeapons = new List<Weapon>();

            foreach(var item in newWeapons)
            {
                Debug.Log("Trying to load: " + item.weaponName);
                DeckManager.instance.unlockedWeapons.Add(item);
            }

            #endregion
        }

        List<string> collectedSpellsSaved;
        List<string> equippedSpellsSaved;
        List<string> collectedWeaponsSaved;
        string equippedWeaponSaved;

        public object CaptureState()
        {
            SaveCards(DeckManager.instance.collection, DeckManager.instance.majorArcana);

            return new SaveData 
            { 
                collectedSpells = this.collectedSpellsSaved,
                equippedSpells = this.equippedSpellsSaved,
                collectedWeapons = this.collectedWeaponsSaved,
                equippedWeapon = this.equippedWeaponSaved
            };
        }

        public void RestoreState(object state)
        {
            var saveData = (SaveData)state;

            collectedSpellsSaved = saveData.collectedSpells;
            equippedSpellsSaved = saveData.equippedSpells;
            collectedWeaponsSaved = saveData.collectedWeapons;
            equippedWeaponSaved = saveData.equippedWeapon;

            LoadCards(DeckManager.instance.collection, DeckManager.instance.majorArcana);
        }

        public void ResetState()
        {
            //TODO: Reset all values to default and then save them
            collectedSpellsSaved = new List<string>();
            equippedSpellsSaved = new List<string>();

            DeckManager.instance.collection.Clear();
            DeckManager.instance.majorArcana.Clear();
            DeckManager.instance.unlockedWeapons.Clear();
            DeckManager.instance.weapon = defaultWeapon;

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

            public List<string> collectedWeapons;
            public string equippedWeapon;
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