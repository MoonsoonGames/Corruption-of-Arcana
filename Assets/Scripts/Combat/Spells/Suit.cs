using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSuit", menuName = "Combat/Suit", order = 0)]
public class Suit : ScriptableObject
{
    public string suitName;
    [TextArea(3, 10)]
    public string suitDescription; // Basic desciption of spell effect

    public bool overrideTarget;
    public bool cleaveTargets;
    public bool chainTargets;
    public bool affectPlayer;

    public E_DamageTypes effectType;
    public float speedMultiplier;

    public void CastSpell(Character target, int value)
    {
        target.GetHealth().ChangeHealth(effectType, value);
    }
}