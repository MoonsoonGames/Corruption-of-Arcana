using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Necropanda.Utils.Console.Commands;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class CardsDataManager : MonoBehaviour
    {
        //C:\Users\as243879\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled
        //C:\Users\mr232432\AppData\LocalLow\Necropanda Studios\CoA 2_ Reshuffled

        #region Singleton
        //Code from last year

        public static CardsDataManager instance = null;

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

        [SerializeField] private GiveCommand giveCommand;

        // Start is called before the first frame update
        void Start()
        {
            Singleton();
            SaveManager.instance.saveAllData += SaveCardsData;
            SaveManager.instance.saveAllBaseData += SaveBaseCardsData;
            SaveManager.instance.overideAllBaseData += OverideBaseCardsData;
            SaveManager.instance.loadAllData += LoadCardsData;
            SaveManager.instance.loadAllBaseData += LoadBaseCardsData;
        }

        [ContextMenu("Save Cards Data")]
        public void SaveCardsData()
        {
            Debug.Log("Saving Cards data");

            CardSaving.SaveCardsData(DeckManager.instance.collection, "Collection");
            CardSaving.SaveCardsData(DeckManager.instance.majorArcana, "MajorArcana");
        }

        [ContextMenu("Save Base Cards Data")]
        public void SaveBaseCardsData()
        {
            Debug.Log("Saving Cards data");

            CardSaving.SaveBaseCardsData(DeckManager.instance.collection, "Collection");
            CardSaving.SaveBaseCardsData(DeckManager.instance.majorArcana, "MajorArcana");
        }

        [ContextMenu("Overide Base Cards Data")]
        public void OverideBaseCardsData()
        {
            Debug.Log("Saving base Cards data");
            CardSaving.SaveBaseCardsData(DeckManager.instance.collection, "Collection");
            CardSaving.SaveBaseCardsData(DeckManager.instance.majorArcana, "MajorArcana");
        }

        [ContextMenu("Load Cards Data")]
        public void LoadCardsData()
        {
            Debug.Log("Loading Cards data");

            CardData collectionData = CardSaving.LoadCardData("/Collection_cards.dat");
            CardData equipData = CardSaving.LoadCardData("/MajorArcana_cards.dat");

            if (collectionData == null || equipData == null) return;

            LoadData(collectionData, equipData);
        }

        [ContextMenu("Load Base Cards Data")]
        public void LoadBaseCardsData()
        {
            Debug.Log("Loading base Cards data");

            CardData collectionData = CardSaving.LoadCardData("/Collection_cardBase.dat");
            CardData equipData = CardSaving.LoadCardData("/MajorArcana_cardBase.dat");

            if (collectionData == null || equipData == null) return;

            LoadData(collectionData, equipData);
        }

        void LoadData(CardData collectionData, CardData equipData)
        {
            if (giveCommand == null)
                return;

            DeckManager.instance.collection.Clear();
            DeckManager.instance.majorArcana.Clear();

            if (collectionData.cardsDict.Count > 0)
            {
                foreach (var item in collectionData.cardsDict)
                {
                    if (giveCommand._objectKeys.Contains(item))
                    {
                        giveCommand.GiveToPlayer(item);
                    }
                }
            }

            if (equipData.cardsDict.Count > 0)
            {
                foreach (var item in equipData.cardsDict)
                {
                    if (giveCommand._objectKeys.Contains(item))
                    {
                        giveCommand.EquipToPlayer(item);
                    }
                }
            }
        }

        [ContextMenu("Reset Cards Data")]
        public void ResetCardsData()
        {
            
        }
    }
}
