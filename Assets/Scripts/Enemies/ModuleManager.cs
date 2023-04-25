using System;
using UnityEngine;
using Necropanda.AI.Movement;
using Necropanda.Utils.Debugger;

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

        private void OnEnable()
        {
            if (!wander || !patrol)
                CheckScripts();
        }

        public void CheckScripts()
        {

            try
            {
                wander = gameObject.GetComponent<Wander>();
            }
            catch (NullReferenceException err)
            {
                Debugger.instance.SendDebug($"No wander module found on this {this.gameObject.name}, adding one..", 3);
                Debugger.instance.SendDebug($"{err.Message}, should be fixed now. Disabling module to avoid errors", 2);
                wander = gameObject.AddComponent<Wander>();
                wander.enabled = false;
            }

            try
            {
                patrol = gameObject.GetComponent<Patrol>();
            }
            catch (NullReferenceException err)
            {
                Debugger.instance.SendDebug($"No patrol module found on this {this.gameObject.name}, adding one..", 3);
                Debugger.instance.SendDebug($"{err.Message}, should be fixed now. Disabling module to avoid errors", 2);
                patrol = gameObject.AddComponent<Patrol>();
                try
                {
                    patrol.StopPatrol();
                }
                catch (NullReferenceException)
                {
                    Debugger.instance.SendDebug("Tried to stop patrol, error occured. Forcefully stopping it.", 2);
                    patrol.enabled = false;
                }
            }

            Debugger.instance.SendDebug($"Had to check scripts on {gameObject.name}, is something unnassgined?", 2);
        }

        /// <summary>
        /// Disables selected modules.
        /// </summary>
        /// <param name="type">Determines what it disables, 1 to disable wander, 2 to disable patrol</param>
        /// <param name="state">determines whether the module is enabled or disabled</param>
        public void ChangeModuleState(int type, bool state)
        {
            switch (type)
            {
                case 1:
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            wander.enabled = state;
                        }
                        catch (NullReferenceException)
                        {
                            Debugger.instance.SendDebug($"Couldn't disable the wandering module, trying again for {i} more time(s)", 3);
                            wander.enabled = false;
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            patrol.enabled = state;
                        }
                        catch (NullReferenceException)
                        {
                            Debugger.instance.SendDebug("Tried to stop the patrol module, error occured. Forcefully stopping it.", 3);
                            patrol.enabled = false;
                        }
                    }
                    break;

                default:
                    Debugger.instance.SendDebug("Invalid type was entered. Please enter 1 or 2. Stopping script to avoid errors.", 3);
                    break;
            }
        }

        /// <summary>
        /// Disables all modules. Pass no args for this functionality.
        /// </summary>
        public void ChangeModuleState()
        {
            ChangeModuleState(1, false);
            ChangeModuleState(2, false);
        }
    }
}