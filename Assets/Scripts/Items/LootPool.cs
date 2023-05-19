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

        public List<Object> RewardItems()
        {
            List<Object> items = new List<Object>();

            foreach (var pool in lootPools)
            {
                if (pool.rewardAll)
                {
                    Debug.Log("Reward all items");
                    foreach(Object item in pool.objects)
                    {
                        items.Add(item);
                        GiveItem(item);
                    }
                }
                else
                {
                    Debug.Log("Reward item - called");
                    int count = Random.Range(pool.number.x, pool.number.y);
                    for (int i = 0; i < count; i++)
                    {
                        Debug.Log("Reward one item");
                        int randInt = Random.Range(0, pool.objects.Count);
                        items.Add(pool.objects[randInt]);
                        GiveItem(pool.objects[randInt]);
                    }
                }
            }

            GiveGold();

            return items;
        }

        void GiveGold()
        {
            //Add gold to player
        }

        void GiveItem(Object item)
        {
            if (!Application.isPlaying)
                return;

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
                        DeckManager.instance.collection.Add(currentSpell);
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
        public Vector2Int number;
        public bool rewardAll;
    }
}