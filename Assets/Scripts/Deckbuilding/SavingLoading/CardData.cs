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
    [System.Serializable]
    public class CardData
    {
        public List<string> cardsDict;
        public string fileName;

        public CardData(List<Spell> cardsToSave, string fileName)
        {
            this.fileName = fileName;
            cardsDict = new List<string>();
            foreach (Spell spell in cardsToSave)
            {
                Debug.Log(spell.name);
                cardsDict.Add(spell.name);
            }

            foreach (var item in cardsDict)
                Debug.Log(item);
        }
    }
}
