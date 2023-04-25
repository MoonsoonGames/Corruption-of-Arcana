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
    public class LootPool
    {
        public List<Object> items;
        public Vector2Int gold;

        public void RewardAllItems()
        {
            foreach (Object item in items)
            {
                GiveItem(item);
            }

            GiveGold();
        }

        public void RewardRandomItem()
        {
            int randInt = Random.Range(0, items.Count);

            GiveItem(items[randInt]);
            GiveGold();
        }

        void GiveGold()
        {
            //Add gold to player
        }

        void GiveItem(Object item)
        {
            if (item.GetType() == typeof(Weapon))
            {
                // Add to deck manager list
                DeckManager.instance.unlockedWeapons.Add((Weapon)item);
            }
            else if (item.GetType() == typeof(Spell))
            {
                Spell currentSpell = (Spell)item;
                switch (currentSpell.cardType)
                {
                    case E_CardTypes.Cards:
                        // Add to deck manager list
                        DeckManager.instance.collection.Add((Spell)item);
                        break;
                    case E_CardTypes.Potions:
                        PotionManager.instance.ChangePotion(currentSpell.potionType, 1);
                        break;
                }
            }
        }
    }
}