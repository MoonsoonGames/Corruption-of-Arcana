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
        //C:\Users\as243879\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        //C:\Users\mr232432\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        static string savePath = Application.persistentDataPath + "/Quests";

        public static void SaveQuestData(List<Quest> questsToSave)
        {
            CreateDirectory(savePath);

            QuestData questData = new QuestData(questsToSave);
            string localPath = savePath + "/" + questData.fileName + "_quest.dat";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(localPath, FileMode.Create);

            formatter.Serialize(stream, questData);

            stream.Close();
        }

        public static QuestData LoadQuestData(string localPath)
        {
            CreateDirectory(savePath);

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
                Debug.LogWarning("Save file not found in " + fullPath);
                return null;
            }
        }

        public static void SaveBaseQuestData(List<Quest> questsToSave)
        {
            CreateDirectory(savePath);

            QuestData questData = new QuestData(questsToSave);
            string localPath = savePath + "/" + questData.fileName + "_questBase.dat";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(localPath, FileMode.Create);

            formatter.Serialize(stream, questData);

            stream.Close();
        }

        public static bool BaseDataExists(List<Quest> questsToSave)
        {
            QuestData questData = new QuestData(questsToSave);
            string localPath = savePath + "/" + questData.fileName + "_questBase.dat";
            return File.Exists(localPath);
        }

        static void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Debug.Log("Directory exists");
            }
            else
            {
                Debug.Log("Directory does not exist -> Creating directory");
                Directory.CreateDirectory(path);
            }
            
        }
    }
}
