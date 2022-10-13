using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    [CreateAssetMenu(fileName = "NewSpell", menuName = "Combat/Spells", order = 0)]
    public class Spell : ScriptableObject
    {
        [Header("Basic Info")]
        public string spellName;
        public bool overrideColor;
        public Color timelineColor;
        [TextArea(3, 10)]
        public string spellDescription; // Basic desciption of spell effect
        public float speed;
        public int arcanaCost;

        [Header("Caster Effects")]
        public E_DamageTypes effectTypeCaster;
        public int valueCaster;
        //Caster status effects here

        [Header("Target Effects")]
        public E_DamageTypes effectTypeTarget;
        public int valueTarget;
        public int hitCount = 1;
        public float executeThreshold;
        public E_MultihitType multihitType;
        public int multihitValue;
        //Target status effects here

        public void CastSpell(Character target, Character caster)
        {
            if (caster != null)
            {
                AffectCaster(caster, effectTypeCaster, valueCaster);
            }
            if (target != null)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = new List<Character>();

                for (int i = 0; i < hitCount; i++)
                {
                    switch (multihitType)
                    {
                        case E_MultihitType.Single:
                            AffectTarget(target, effectTypeTarget, valueTarget);
                            break;
                        case E_MultihitType.Chain:
                            foreach (Character character in targetTeamManager.team)
                            {
                                if (character != target)
                                {
                                    AffectTarget(target, effectTypeTarget, multihitValue);
                                }
                                else
                                {
                                    character.GetHealth().ChangeHealth(effectTypeTarget, valueTarget);
                                }
                            }
                            break;
                        case E_MultihitType.Cleave:
                            foreach (Character character in targetTeamManager.team)
                            {
                                if (character != target)
                                {
                                    AffectTarget(target, effectTypeTarget, multihitValue);
                                }
                                else
                                {
                                    character.GetHealth().ChangeHealth(effectTypeTarget, valueTarget);
                                }
                            }
                            break;
                        case E_MultihitType.RandomTeam:
                            AffectTarget(targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)], effectTypeTarget, valueTarget);
                            break;
                        case E_MultihitType.RandomAll:
                            allCharacters = CombineLists(targetTeamManager.team, casterTeamManager.team);
                            AffectTarget(allCharacters[Random.Range(0, allCharacters.Count)], effectTypeTarget, valueTarget);
                            break;
                        case E_MultihitType.All:
                            allCharacters = CombineLists(targetTeamManager.team, casterTeamManager.team);
                            foreach (Character character in allCharacters)
                            {
                                AffectTarget(character, effectTypeTarget, valueTarget);
                            }
                            break;
                    }
                }
            }
        }

        void AffectCaster(Character target, E_DamageTypes effectType, int value)
        {
            //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
            target.GetHealth().ChangeHealth(effectType, value);
        }

        void AffectTarget(Character target, E_DamageTypes effectType, int value)
        {
            //Debug.Log("Affect " + target.characterName + " with " + value + " " + effectType);
            target.GetHealth().ChangeHealth(effectType, value);

            if (target.GetHealth().GetHealthPercentage() < executeThreshold)
            {
                Debug.Log("Kill " + target.characterName + " with " + name);
                target.GetHealth().ChangeHealth(E_DamageTypes.Perforation, 9999999);
            }
        }

        public List<Character> CombineLists(List<Character> list1, List<Character> list2)
        {
            List<Character> outputList = new List<Character>();

            foreach (Character character in list1)
            {
                outputList.Add(character);
            }
            foreach (Character character in list2)
            {
                outputList.Add(character);
            }

            return outputList;
        }
    }
}
