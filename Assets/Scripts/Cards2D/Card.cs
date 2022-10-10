using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    [HideInInspector]
    public Spell spell;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI descriptionText;

    public void Setup()
    {
        nameText.text = spell.spellName;
        speedText.text = spell.speed.ToString();
        descriptionText.text = spell.spellDescription;

        gameObject.name = spell.spellName;
    }

    public void CastSpell(Character target, Character caster)
    {
        spell.CastSpell(target, caster);
    }
}
