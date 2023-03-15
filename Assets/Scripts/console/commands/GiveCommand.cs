using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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
            // The second is the amount to be given
            int amountToGive = int.Parse(args[1]);



            // Check that the argument is in the dictionary
            if (!IsValidObject(objToGive))
            {
                // if its not we return an error to the console
                DeveloperConsoleBehaviour.OutputMessage = $"Object: \"{objToGive}\" isn't valid!";
            }

            // Check to see what message we need to pass to the console, and what next action we need to take
            if (CheckArgsAmount(args) == 1)
            {
                // if it is, we output the correct message to the console
                DeveloperConsoleBehaviour.OutputMessage = $"Giving the player a {objToGive}";
                // Do stuff to add the item
            }
            else
            {
                DeveloperConsoleBehaviour.OutputMessage = $"Giving the player [{amountToGive}] {objToGive}";
                // Do stuff to add more than one item
            }
            throw new System.NotImplementedException();
        }


        private int CheckArgsAmount(string[] args)
        {
            return args.Length;
        }

        private bool IsValidObject(string objectToCheckName)
        {
            if (objects.ContainsKey(objectToCheckName))
            {
                return true;
            }
            else
            {
                return false;
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
                Debug.Log("You have " + items.Value + " " + items.Key);
            }
        }

    }
}
