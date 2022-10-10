using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Combat/Spells", order = 0)]
public class Spell : ScriptableObject
{
    public string spellName;
    public Color timelineColor;
    [TextArea(3, 10)]
    public string spellDescription; // Basic desciption of spell effect

    public bool overrideTarget;
    public bool cleaveTargets;
    public bool chainTargets;
    public bool affectPlayer;

    public E_DamageTypes effectType;
    public int value;
    public float speed;

    public void CastSpell(Character target, Character caster)
    {
        if (target != null)
        {
            target.GetHealth().ChangeHealth(effectType, value);
        }
    }
}