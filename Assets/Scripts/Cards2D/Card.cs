using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public Spell spell;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void Setup()
    {
        
        nameText.text = spell.spellName;
        descriptionText.text = spell.spellDescription;

        gameObject.name = spell.spellName;
    }

    public void CastSpell(Character target)
    {
        spell.CastSpell(target, null);
    }
}
