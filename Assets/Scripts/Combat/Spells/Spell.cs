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
        public string spellName;
        public Color timelineColor;
        [TextArea(3, 10)]
        public string spellDescription; // Basic desciption of spell effect

        public E_DamageTypes effectTypeCaster;
        public int valueCaster;
        //Caster status effects here

        public E_DamageTypes effectTypeTarget;
        public int valueTarget;
        public int hitCount = 1;
        public E_MultihitType multihitType;
        public int multihitValue;
        //Target status effects here

        public float speed;
        public int arcanaCost;

        public void CastSpell(Character target, Character caster)
        {
            if (caster != null)
            {
                caster.GetHealth().ChangeHealth(effectTypeCaster, valueCaster);
            }
            if (target != null)
            {
                TeamManager targetTeamManager = target.GetManager();
                TeamManager casterTeamManager = caster.GetManager();
                List<Character> allCharacters = new List<Character>();

                for (int i = 0; i < hitCount; i++)
                {
                    if (multihitType != E_MultihitType.Single)
                    {
                        switch (multihitType)
                        {
                            case E_MultihitType.Single:
                                target.GetHealth().ChangeHealth(effectTypeTarget, valueTarget);
                                break;
                            case E_MultihitType.Chain:
                                foreach(Character character in targetTeamManager.team)
                                {
                                    if (character != target)
                                    {
                                        character.GetHealth().ChangeHealth(effectTypeTarget, multihitValue);
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
                                        character.GetHealth().ChangeHealth(effectTypeTarget, multihitValue);
                                    }
                                    else
                                    {
                                        character.GetHealth().ChangeHealth(effectTypeTarget, valueTarget);
                                    }
                                }
                                break;
                            case E_MultihitType.RandomTeam:
                                targetTeamManager.team[Random.Range(0, targetTeamManager.team.Count)].GetHealth().ChangeHealth(effectTypeTarget, valueTarget);
                                break;
                            case E_MultihitType.RandomAll:
                                allCharacters = CombineLists(targetTeamManager.team, casterTeamManager.team);
                                allCharacters[Random.Range(0, allCharacters.Count)].GetHealth().ChangeHealth(effectTypeTarget, valueTarget);
                                break;
                            case E_MultihitType.All:
                                allCharacters = CombineLists(targetTeamManager.team, casterTeamManager.team);
                                foreach (Character character in allCharacters)
                                {
                                    character.GetHealth().ChangeHealth(effectTypeTarget, valueTarget);
                                }
                                break;


                        }
                    }
                }
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
