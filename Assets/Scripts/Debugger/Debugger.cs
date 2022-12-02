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

        private void Start()
        {
            Singleton();
        }

        /// <summary>
        /// Function that collects and sends the correct message into the console.
        /// </summary>
        /// <param name="message">The message to be sent</param>
        public void SendDebug(string message)
        {
            switch (debugLevel)
            {
                case DebuggerState.None:
                    break;
                case DebuggerState.Log:
                    Debug.Log(message);
                    break;
                case DebuggerState.Warning:
                    Debug.LogWarning(message);
                    break;
                case DebuggerState.Error:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}
