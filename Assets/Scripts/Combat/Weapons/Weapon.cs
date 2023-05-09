using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "NewWeapon", menuName = "Combat/Weapons", order = 4)]
    public class Weapon : ScriptableObject
    {
        public string weaponName;
        [TextArea(3, 10)]
        public string description;
        public Sprite image;
        public int power;
        public List<Spell> spells;
        public List<Spell> upgradeSpells;

        public void Equip()
        {
            if (DeckManager.instance == null) { return; }

            DeckManager.instance.weapon = this;
        }

        [ContextMenu("Upgrade1Tier")]
        public void UpgradeCards()
        {
            upgradeSpells.Clear();

            foreach(var item in spells)
            {
                upgradeSpells.Add(item.GetUpgrade());
            }
        }
    }
}