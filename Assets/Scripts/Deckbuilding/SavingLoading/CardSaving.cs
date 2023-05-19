using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public static class CardSaving
    {
        //C:\Users\as243879\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        //C:\Users\mr232432\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        static string savePath = Application.persistentDataPath + "/Cards";

        public static void SaveCardsData(List<Spell> spellsToSave, string fileName)
        {
            CreateDirectory(savePath);

            CardData cardData = new CardData(spellsToSave, fileName);
            string localPath = savePath + "/" + cardData.fileName + "_cards.dat";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(localPath, FileMode.Create);

            formatter.Serialize(stream, cardData);

            stream.Close();
        }

        public static CardData LoadCardData(string localPath)
        {
            CreateDirectory(savePath);

            string fullPath = savePath + localPath;
            if (File.Exists(fullPath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(fullPath, FileMode.Open);

                CardData cardData = formatter.Deserialize(stream) as CardData;
                stream.Close();

                return cardData;
            }
            else
            {
                Debug.LogWarning("Save file not found in " + fullPath);
                return null;
            }
        }

        public static void SaveBaseCardsData(List<Spell> spellsToSave, string fileName)
        {
            Debug.Log("saving base data");
            CreateDirectory(savePath);

            CardData cardData = new CardData(spellsToSave, fileName);
            string localPath = savePath + "/" + cardData.fileName + "_cardBase.dat";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(localPath, FileMode.Create);

            formatter.Serialize(stream, cardData);

            stream.Close();
        }

        public static bool BaseDataExists(List<Spell> spellsToSave, string fileName)
        {
            CardData cardData = new CardData(spellsToSave, fileName);
            string localPath = savePath + "/" + cardData.fileName + "_cardBase.dat";
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
