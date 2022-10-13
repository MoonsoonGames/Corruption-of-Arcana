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
            if (target != null)
            {
                TeamManager teamManager = target.GetManager();
                for (int i = 0; i < hitCount; i++)
                {
                    target.GetHealth().ChangeHealth(effectTypeTarget, valueTarget);

                    if (multihitType != E_MultihitType.Single)
                    {
                        List<Character> targets = teamManager.team;

                        switch (multihitType)
                        {
                            case (E_MultihitType.Chain):
                                foreach(Character character in targets)
                                {
                                    if (character != target)
                                    {
                                        character.GetHealth().ChangeHealth(effectTypeTarget, multihitValue);
                                    }
                                }
                                break;
                            case (E_MultihitType.Cleave):
                                foreach (Character character in targets)
                                {
                                    if (character != target)
                                    {
                                        character.GetHealth().ChangeHealth(effectTypeTarget, multihitValue);
                                    }
                                }
                                break;

                        }
                    }
                }
            }
        }
    }
}
