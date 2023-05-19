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
    [CreateAssetMenu(fileName = "New Change Quest Progress Command", menuName = "Utilities/DeveloperConsole/Commands/CQP Command")]
    /// <summary>
    /// Allows the player to add items to thier inventory using the console
    /// </summary>
    public class ChangeQuestProgressCommand : ConsoleCommand, ISerializationCallbackReceiver
    {
        // Unity doesn't know how to serialize dictionaries, so we make lists to show them in the inspector
        [Header("Quests:")]
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
            Debug.Log("RUNNING");
            // The first argument is the item to be given
            var questToChange = args[0];

            // Check that the argument is in the dictionary
            if (!IsValidQuest(questToChange))
            {
                // if its not we return an error to the console
                Debug.Log("Not valid quest!");
                DeveloperConsoleBehaviour.OutputMessage = $"Quest: \"{questToChange}\" isn't valid!";
                DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
                developerConsoleBehaviour.UpdateOutputMessage();
                return false;
            }

            // Check to see what message we need to pass to the console, and what next action we need to take
            if (CheckArgsAmount(args) > 1)
            {
                // The second is the amount to be given
                int stageInt = int.Parse(args[1]);

                Debug.Log("Args > 1");
                DeveloperConsoleBehaviour.OutputMessage = $"Progressing quest [{questToChange}] to stage {stageInt}";
                DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
                developerConsoleBehaviour.UpdateOutputMessage();

                GiveToPlayer(questToChange, stageInt);

                return true;
            }
            else
            {
                // if its not we return an error to the console
                Debug.Log("Not enough params given in quest command");
                DeveloperConsoleBehaviour.OutputMessage = $"Not enough params given! (FORMAT: [\"QUESTNAME\" \"QUESTSTAGE\"])";
                DeveloperConsoleBehaviour developerConsoleBehaviour = GameObject.FindObjectOfType<DeveloperConsoleBehaviour>();
                developerConsoleBehaviour.UpdateOutputMessage();
                return false;
            }
        }


        private int CheckArgsAmount(string[] args)
        {
            return args.Length;
        }

        private bool IsValidQuest(string objectToCheckName)
        {
            if (objects.ContainsKey(objectToCheckName))
            {
                Debug.Log("TRUE");
                return true;
            }
            else
            {
                Debug.Log($"FALSE: {objectToCheckName}");
                return false;
            }
        }

        /// <summary>
        /// QUEST STAGES:
        /// C1M 13 - Cave quest start
        /// </summary>
        /// <param name="questName">The name of the quest</param>
        /// <param name="stage">The stage that the quest should be changed to</param>
        private void GiveToPlayer(string questName, int stage)
        {
            if (objects[questName].GetType() == typeof(Quest))
            {
                // Add to deck manager list
                Quest quest = (Quest)objects[questName];
                quest.DevForceSetQuestProgress(stage);
            }

        }


        [ContextMenu("Find Quests")]
        /// <summary>
        /// Finds any Quests across the project and correctly adds them to the dictionary. 
        /// Ideally should be ran before every build
        /// </summary>
        private void FindQuests()
        {
            //Make sure all resources are loaded
            var quests = Resources.LoadAll("Quests", typeof(Quest));

            // clear the dictionary
            objects = new Dictionary<string, UnityEngine.Object>();

            // Find the items
            Quest[] questList = Resources.FindObjectsOfTypeAll<Quest>();

            Debug.Log($"Quest length: {questList.Length}");

            // add all quests to the dictionary
            foreach (Quest quest in questList)
            {
                objects.Add(quest.name, quest);
                Debug.Log(quest.name);
            }
        }

        [ContextMenu("Log Dictionary Items")]
        private void LogQuestsInDictionary()
        {
            foreach (KeyValuePair<string, UnityEngine.Object> items in objects)
            {
                Debug.Log("You have " + items.Value + ": " + items.Key);
            }
        }

    }
}
