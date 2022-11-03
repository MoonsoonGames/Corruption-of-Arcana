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
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Combat/Character Stats", order = 2)]
    public class CharacterStats : ScriptableObject
    {
        public string characterName;
        public Sprite characterSprite;
        public string characterDescription;
        public string location;

        public int maxHealth;
        public E_DamageTypes[] baseDamageResistancesType;
        public float[] baseDamageResistancesModifier;

        public Color timelineColor = new Color(0, 0, 0, 255);

        public SpellCastingAI ai;
        public bool aiSpawnsCards;
        public List<CombatHelperFunctions.AISpell> aISpells = new List<CombatHelperFunctions.AISpell>();
    }
}
