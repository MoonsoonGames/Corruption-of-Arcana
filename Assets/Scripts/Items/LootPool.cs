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
        public List<LootPools> lootPools;
        public Vector2Int gold;

        public void RewardItems()
        {
            foreach (var pool in lootPools)
            {
                if (pool.rewardAll)
                {
                    foreach(Object item in pool.objects)
                    {
                        GiveItem(item);
                    }
                }
                else
                {
                    for (int i = 0; i < pool.objects.Count; i++)
                    {
                        int randInt = Random.Range(0, pool.objects.Count);
                        GiveItem(pool.objects[randInt]);
                    }
                }
            }

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

    [System.Serializable]
    public struct LootPools
    {
        public List<Object> objects;
        public int count;
        public bool rewardAll;
    }
}