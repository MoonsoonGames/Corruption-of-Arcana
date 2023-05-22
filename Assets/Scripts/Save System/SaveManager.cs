using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SaveManager : MonoBehaviour
    {
        //C:\Users\as243879\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        //C:\Users\mr232432\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled

        #region Singleton
        //Code from last year

        public static SaveManager instance = null;

        void Singleton()
        {
            if (instance == null)
            {
                instance = this;

                gameObject.transform.SetParent(null);
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        public delegate void Delegate();
        public Delegate saveAllData, loadAllData, resetAllData;

        private void Start()
        {
            Singleton();
        }

        public void SaveAllData()
        {
            //Saves data
            saveAllData();
        }
        public void LoadAllData()
        {
            //Loads data
            loadAllData();
        }

        public void ResetAllData()
        {
            //Reverts all data to their base values and deletes the save file
            ResetAllData();
        }
    }
}
