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
        public Delegate saveAllData, saveAllBaseData, loadAllData, loadAllBaseData;

        private void Start()
        {
            Singleton();
        }

        public void SaveAllData()
        {
            saveAllData();
        }

        public void SaveAllBaseData()
        {
            saveAllBaseData();
        }

        public void LoadAllData()
        {
            loadAllData();
        }

        public void LoadAllBaseData()
        {
            loadAllBaseData();
        }
    }
}
