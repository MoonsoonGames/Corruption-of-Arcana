using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpellInstance
{
    public Spell spell;
    public Character caster;
    public Character target;

    public void SetSpellInstance(Spell newSpell, Character newTarget, Character newCaster)
    {
        spell = newSpell;
        target = newTarget;
        caster = newCaster;
    }
}

public interface IInteractable
{
    void Interacted(GameObject player);
}