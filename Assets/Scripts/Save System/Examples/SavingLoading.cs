using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by @mattordev using Dino Dappers tutorial - https://www.youtube.com/watch?v=f5GvfZfy3yk&t=2s
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// 
/// TODO:
/// - Implement crash backup saving to prevent data corruption:
///     before you write the save file, check if it exists, copy it, change extension to .bak
///     save the new data and delete the .bak. 
///     on load you can check for a .bak file, check if the date is newer, if so, rename current sav to .temp, 
///         restore the .bak and recover the data
///     delete the temp file.
/// 
/// - Refactor so a binary formatter isn't used.
/// </summary>
namespace Necropanda.SaveSystem
{
    /// <summary>
    /// This class contains all the logic for saving and loading. Uses unitys persistant
    /// data path to save files and load them
    /// 
    /// Currently utilizes a binary formatter but this will be removed in the future.
    /// </summary>
    public class SavingLoading : MonoBehaviour
    {
        #region Singleton
        //Code from last year

        public static SavingLoading instance = null;

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
                //Destroy(gameObject);
            }
        }

        #endregion

        private void Start()
        {
            Singleton();
        }

        private string SavePath => $"{Application.persistentDataPath}/save.dat";

        /// <summary>
        /// Saves the games state to a save file in unitys persistant data path
        /// </summary>
        [ContextMenu("Save")]
        public void Save()
        {
            //Debug.Log("saving");
            var state = LoadFile();
            CaptureState(state);
            SaveFile(state);
        }

        /// <summary>
        /// Loads the save file from unitys persistant data path
        /// </summary>
        [ContextMenu("Load")]
        public void Load()
        {
            //Debug.Log("loading");
            var state = LoadFile();
            RestoreState(state);
        }

        public void ResetData()
        {
            Debug.Log("Reset data called");
            DeleteSaveData();

            ResetState();
        }

        /// <summary>
        /// Deletes the save file
        /// </summary>
        [ContextMenu("Delete Save Data")]
        public void DeleteSaveData()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }

        /// <summary>
        /// Dictionary class used for setting up the save file.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> LoadFile()
        {
            if (!File.Exists(SavePath))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(SavePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Save file class, this creates the save file using a BF
        /// </summary>
        /// <param name="state"></param>
        private void SaveFile(object state)
        {
            using (var stream = File.Open(SavePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        /// <summary>
        /// This function captures the "state" or progress of all tracked metrics in the game.
        /// </summary>
        /// <param name="state"></param>
        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.Id] = saveable.CaptureState();
            }
        }

        /// <summary>
        /// This loads all tracked metrics from disk.
        /// </summary>
        /// <param name="state"></param>
        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }

        private void ResetState()
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveable.ResetState();
            }
        }
    }
}
