using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public class GiveCommand : ConsoleCommand
    {
        [Header("Giveable Objects:")]
        public Dictionary<string, object> objects;

        public override bool Process(string[] args)
        {
            // The first argument is the item to be given
            var objToGive = args[0];
            // The second is the amount to be given
            int amountToGive = int.Parse(args[1]);

            // Check that the argument is in the dictionary
            if (IsValidObject(objToGive))
            {
                // Check to see what message we need to pass to the console, and what next action we need to take
                if (CheckArgsAmount(args) == 1)
                {
                    DeveloperConsoleBehaviour.OutputMessage = $"Giving the player a {objToGive}";

                }
                else
                {
                    DeveloperConsoleBehaviour.OutputMessage = $"Giving the player [{amountToGive}] {objToGive}";
                }
            }

            // if it is, we output the correct message to the console

            // if its not we return an error to the console





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


        [ContextMenu("Find Items")]
        /// <summary>
        /// Finds any giveable items across the project and correctly adds them to the dictionary. 
        /// Ideally should be ran before every build
        /// </summary>
        private void FindGiveableObjects()
        {
            Weapon[] weaponsList = Resources.FindObjectsOfTypeAll<Weapon>();
            Card[] cardList = Resources.FindObjectsOfTypeAll<Card>();

            // add all weapons to the dictionary
            foreach (Weapon weapon in weaponsList)
            {
                objects.Add(weapon.name, weapon);
            }
            // add all Cards to the dictionary
            foreach (Card card in cardList)
            {
                objects.Add(card.name, card);
            }
        }

    }
}
