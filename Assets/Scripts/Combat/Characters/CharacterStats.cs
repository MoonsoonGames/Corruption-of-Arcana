using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Combat/Character Stats", order = 2)]
    public class CharacterStats : ScriptableObject
    {
        public string characterName;
        public Object characterObject;
        public Sprite characterSprite;
        public string characterDescription;
        public string location;

        public int maxHealth;
        public int startingShields;
        public bool decayShields;
        public E_DamageTypes[] baseDamageResistancesType;
        public float[] baseDamageResistancesModifier;

        public Color timelineColor = new Color(0, 0, 0, 255);
        public Object spawnObject;

        public SpellCastingAI ai;
        public int actions = 1;
        public bool usesArcana;
        public List<CombatHelperFunctions.AISpell> aISpells = new List<CombatHelperFunctions.AISpell>();
    }
}
