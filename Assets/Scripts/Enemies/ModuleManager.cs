using System;
using UnityEngine;
using Necropanda.AI.Movement;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com and @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.AI
{
    public class ModuleManager : MonoBehaviour
    {
        public Wander wander;
        public Patrol patrol;

        private EnemyAI aiController;

        private void Awake()
        {
            aiController = GetComponent<EnemyAI>();
        }

        private void OnEnable() {
            CheckScripts();
        }

        public void CheckScripts()
        {
            //Debug.Log("Checking Enemy AI scripts on " + gameObject.name);
            try
            {
                wander = gameObject.GetComponent<Wander>();
            }
            catch (NullReferenceException err)
            {
                Debug.LogError($"No wander module found on this {this.gameObject.name}, adding one..");
                Debug.LogWarning($"{err.Message}, should be fixed now. Disabling module to avoid errors");
                wander = gameObject.AddComponent<Wander>();
                wander.enabled = false;
            }

            try
            {
                patrol = gameObject.GetComponent<Patrol>();
            }
            catch (NullReferenceException err)
            {
                Debug.LogError($"No patrol module found on this {this.gameObject.name}, adding one..");
                Debug.LogWarning($"{err.Message}, should be fixed now. Disabling module to avoid errors");
                patrol = gameObject.AddComponent<Patrol>();
                try
                {
                    patrol.StopPatrol();
                }
                catch (NullReferenceException)
                {
                    Debug.LogWarningFormat("Tried to stop patrol, error occured. Forcefully stopping it.");
                    patrol.enabled = false;
                }
            }
        }

        /// <summary>
        /// Needs additional work, avoid passing in a true bool for now.
        /// </summary>
        /// <param name="type">Determines what it disables, 1 disable AI movement modules or 2 disable mouse modules</param>
        /// <param name="state">determines whether the AI is enabled or disabled</param>
        public void ChangeAllModuleStates(int type, bool state)
        {
            aiController.currentState = AIState.Nothing;
            switch (type)
            {
                case 1:
                DisableAllAIMovementModules:
                    try
                    {
                        wander.enabled = state;
                    }
                    catch (NullReferenceException)
                    {
                        Debug.LogError("Couldn't disable the wandering module, trying again...");
                        wander.enabled = false;
                        goto DisableAllAIMovementModules; // Not sure if this is a good way to go about it..
                    }

                    try
                    {
                        patrol.enabled = state;
                    }
                    catch (NullReferenceException)
                    {
                        Debug.LogError("Tried to stop the patrol module, error occured. Forcefully stopping it.");
                        patrol.enabled = false;
                        goto DisableAllAIMovementModules; // Not sure if this is a good way to go about it..
                    }
                    break;
                default:
                    Debug.LogError("Invalid type was entered. Please enter 1 or 2. Stopping script to avoid errors.");
                    break;
            }
        }
    }
}