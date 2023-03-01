using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK> - Following https://www.youtube.com/watch?v=XOjd_qU2Ido
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public static class QuestSaving
    {
        static string savePath = Application.persistentDataPath;

        public static void SaveQuestData(List<Quest> questsToSave)
        {
            QuestData questData = new QuestData(questsToSave);
            string localPath = savePath + "/" + questData.fileName + "_quest.dat";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(localPath, FileMode.Create);

            formatter.Serialize(stream, questData);

            stream.Close();
        }

        public static QuestData LoadQuestData(string localPath)
        {
            string fullPath = savePath + localPath;
            if (File.Exists(fullPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(fullPath, FileMode.Open);

                QuestData questData = formatter.Deserialize(stream) as QuestData;
                stream.Close();

                return questData;
            }
            else
            {
                Debug.LogError("Save file not found in " + fullPath);
                return null;
            }
        }
    }
}
