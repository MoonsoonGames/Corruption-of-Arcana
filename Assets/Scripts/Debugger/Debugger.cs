using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Authored & Written by @Mattordev, singleton pattern by Andrew Scott
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.Utils.Debugger
{
    /// <summary>
    /// Simple script for allowing the toggling of debugging.
    /// 
    /// TODO: Add debug rate
    /// </summary>
    public class Debugger : MonoBehaviour
    {
        public DebuggerState debugLevel;

        #region Singleton
        //Code from last year

        public static Debugger instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        private void Awake()
        {
            Singleton();
        }

        /// <summary>
        /// Function that collects and sends the correct message into the console.
        /// </summary>
        /// <param name="message">The message to be sent</param>
        public void SendDebug(string message, int priority)
        {
            if (priority != (int)debugLevel && debugLevel != DebuggerState.All)
            {
                return;
            }

            switch (priority)
            {
                case 3:
                    Debug.Log(message);
                    break;
                case 2:
                    Debug.LogWarning(message);
                    break;
                case 1:
                    Debug.LogError(message);
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// Function that collects and sends the correct message into the console with the lowest priority (log).
        /// </summary>
        /// <param name="message">The message to be sent</param>
        public void SendDebug(string message)
        {
            SendDebug(message, 3);
        }
    }
}
