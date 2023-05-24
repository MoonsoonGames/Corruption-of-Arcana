using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Necropanda.Utils.Console;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Console.Commands
{
    [CreateAssetMenu(fileName = "New Give Command", menuName = "Utilities/DeveloperConsole/Commands/Give Command")]
    /// <summary>
    /// Allows the player to add items to thier inventory using the console
    /// </summary>
    public class GiveCommand : ConsoleCommand, ISerializationCallbackReceiver
    {
        // Unity doesn't know how to serialize dictionaries, so we make lists to show them in the inspector
        [Header("Giveable Objects:")]
        public List<string> _objectKeys = new List<string>();
        public List<UnityEngine.Object> _objectValues = new List<UnityEngine.Object>();

        public Dictionary<string, UnityEngine.Object> objects;

        #region Dictionary Serialsation Work around
        public void OnBeforeSerialize()
        {
            _objectKeys.Clear();
            _objectValues.Clear();

            foreach (var kvp in objects)
            {
                _objectKeys.Add(kvp.Key);
                _objectValues.Add(kvp.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            objects = new Dictionary<string, UnityEngine.Object>();

            for (int i = 0; i != Math.Min(_objectKeys.Count, _objectValues.Count); i++)
                objects.Add(_objectKeys[i], _objectValues[i]);
        }

        void OnGUI()
        {
            foreach (var kvp in objects)
                GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
        }
        #endregion

        public override bool Process(string[] args)
        {
            // The first argument is the item to be given
            var objToGive = args[0];

            // Check that the argument is in the dictionary
            if (!IsValidObject(objToGive))
            {
                // if its not we return an error to the console
                Debug.Log("Not valid object!");
                DeveloperConsoleBehaviour.OutputMessage = $"Object: \"{objToGive}\" isn't valid!";
                DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
                developerConsoleBehaviour.UpdateOutputMessage();
                return false;
            }

            // Check to see what message we need to pass to the console, and what next action we need to take
            if (CheckArgsAmount(args) == 1)
            {
                // if it is, we output the correct message to the console
                Debug.Log("Args = 1");
                DeveloperConsoleBehaviour.OutputMessage = $"Giving the player a {objToGive}";

                DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
                developerConsoleBehaviour.UpdateOutputMessage();

                // Do stuff to add the item
                // Check item type
                GiveToPlayer(objToGive);

                return true;
            }
            else
            {
                if (objToGive == "Gold")
                {
                    GoldManager.instance.AddGold(int.Parse(args[1]));
                }


                // The second is the amount to be given
                int amountToGive = int.Parse(args[1]);

                Debug.Log("Args > 1");
                DeveloperConsoleBehaviour.OutputMessage = $"Giving the player [{amountToGive}] {objToGive}";
                DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
                developerConsoleBehaviour.UpdateOutputMessage();
                // Do stuff to add more than one item

                for (int amt = 0; amt < amountToGive; amt++)
                {
                    GiveToPlayer(objToGive);
                }
                return true;
            }
        }


        private int CheckArgsAmount(string[] args)
        {
            return args.Length;
        }

        public bool IsValidObject(string objectToCheckName)
        {
            if (objects.ContainsKey(objectToCheckName))
            {
                return true;
            }
            else if (objectToCheckName == "Gold")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GiveToPlayer(string objectName)
        {
            //Debug.Log(objectName);
            if (objects[objectName].GetType() == typeof(Weapon))
            {
                // Add to deck manager list
                DeckManager.instance.AddWeapon((Weapon)objects[objectName]);
            }
            else if (objects[objectName].GetType() == typeof(Spell))
            {
                Spell currentSpell = (Spell)objects[objectName];
                Debug.Log(currentSpell.name);
                switch (currentSpell.cardType)
                {
                    case E_CardTypes.Cards:
                        // Add to deck manager list
                        DeckManager.instance.AddCollection((Spell)objects[objectName]);
                        break;
                    case E_CardTypes.Potions:
                        PotionManager.instance.ChangePotion(currentSpell.potionType, 1);
                        break;
                }
            }
        }

        /// <summary>
        /// Second implementation used in saving and loading to avoid duplicate giving of cards.
        /// DO NOT USE FOR ANYTHING ELSE
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="s"></param>
        // public void GiveToPlayer(string objectName, string s)
        // {
        //     if (objects[objectName].GetType() == typeof(Weapon))
        //     {
        //         // Add to deck manager list
        //         DeckManager.instance.unlockedWeapons.Add((Weapon)objects[objectName]);
        //     }
        //     else if (objects[objectName].GetType() == typeof(Spell))
        //     {
        //         // CHECK TO SEE IF CARD IS ALREAY THERE.
        //         // IF IT IS, RETURN.
        //         Spell currentSpell = (Spell)objects[objectName];
        //         switch (currentSpell.cardType)
        //         {
        //             case E_CardTypes.Cards:
        //                 // Add to deck manager list

        //                 DeckManager.instance.collection.Add((Spell)objects[objectName]);
        //                 break;
        //             case E_CardTypes.Potions:
        //                 PotionManager.instance.ChangePotion(currentSpell.potionType, 1);
        //                 break;
        //         }
        //     }
        // }

        public void EquipToPlayer(string objectName)
        {
            if (objects[objectName].GetType() == typeof(Weapon))
            {
                // Add to deck manager list
                DeckManager.instance.EquipWeapon((Weapon)objects[objectName]);
            }
            else if (objects[objectName].GetType() == typeof(Spell))
            {
                Spell currentSpell = (Spell)objects[objectName];
                switch (currentSpell.cardType)
                {
                    case E_CardTypes.Cards:
                        // Add to deck manager list
                        DeckManager.instance.AddMajorArcana((Spell)objects[objectName]);
                        break;
                    case E_CardTypes.Potions:
                        PotionManager.instance.ChangePotion(currentSpell.potionType, 1);
                        break;
                }
            }
        }

        [ContextMenu("Find Givable Items")]
        /// <summary>
        /// Finds any giveable items across the project and correctly adds them to the dictionary. 
        /// Ideally should be ran before every build
        /// </summary>
        private void FindGiveableObjects()
        {
            //Make sure all resources are loaded
            var weapons = Resources.LoadAll("Weapons", typeof(Weapon));
            var cards = Resources.LoadAll("Spells", typeof(Spell));

            // clear the dictionary
            objects = new Dictionary<string, UnityEngine.Object>();

            // Find the items
            Weapon[] weaponsList = Resources.FindObjectsOfTypeAll<Weapon>();
            Spell[] cardList = Resources.FindObjectsOfTypeAll<Spell>();

            Debug.Log($"Weapons length: {weaponsList.Length}");
            Debug.Log($"Cards length: {cardList.Length}");

            // add all weapons to the dictionary
            foreach (Weapon weapon in weaponsList)
            {
                objects.Add(weapon.name, weapon);
                Debug.Log(weapon.name);
            }
            // add all Cards to the dictionary
            foreach (Spell card in cardList)
            {
                objects.Add(card.name, card);
                Debug.Log(card.name);
            }
        }

        [ContextMenu("Log Dictionary Items")]
        private void LogGiveableItemsDictionary()
        {
            foreach (KeyValuePair<string, UnityEngine.Object> items in objects)
            {
                Debug.Log("You have " + items.Value + ": " + items.Key);
            }
        }

    }
}
