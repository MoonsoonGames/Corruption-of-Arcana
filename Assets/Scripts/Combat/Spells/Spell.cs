using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Combat/Spells", order = 0)]
public class Spell : ScriptableObject
{
    public string spellName;
    [TextArea(3, 10)]
    public string spellDescription; // Basic desciption of spell effect

    public bool overrideTarget;
    public bool cleaveTargets;
    public bool chainTargets;
    public bool affectPlayer;

    public E_DamageTypes effectType;
    public int value;

    public void CastSpell(Character target, Character caster)
    {
        target.GetHealth().ChangeHealth(effectType ,value);
    }
}