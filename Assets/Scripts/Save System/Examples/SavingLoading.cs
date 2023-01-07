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
    public class SavingLoading : MonoBehaviour
    {
        private string SavePath => $"{Application.persistentDataPath}/save.dat";

        [ContextMenu("Save")]
        public void Save()
        {
            Debug.Log("saving");
            var state = LoadFile();
            CaptureState(state);
            SaveFile(state);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            Debug.Log("loading");
            var state = LoadFile();
            RestoreState(state);
        }

        [ContextMenu("Delete Save Data")]
        private void DeleteSaveData()
        {
            if (File.Exists(SavePath))
            {
                File.Delete(SavePath);
            }
        }

        private Dictionary<string, object> LoadFile()
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

        private void SaveFile(object state)
        {
            using (var stream = File.Open(SavePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.Id] = saveable.CaptureState();
            }
        }

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
    }
}
