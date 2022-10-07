using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterHealth : MonoBehaviour
{
    //Health Values
    public int maxHealth;
    protected int health;
    protected int shield;

    //Damage Resistances
    Dictionary<E_DamageTypes, float> baseDamageResistances;
    public E_DamageTypes[] baseDamageResistancesType;
    public float[] baseDamageResistancesModifier;
    Dictionary<E_DamageTypes, float> currentDamageResistances;

    private void Start()
    {
        SetupHealth();
        SetupResistances();
    }

    protected virtual void SetupHealth()
    {
        health = maxHealth;
    }

    protected virtual void SetupResistances()
    {
        baseDamageResistances = new Dictionary<E_DamageTypes, float>();

        for (int i = 0; i < baseDamageResistancesType.Length; i++)
        {
            baseDamageResistances.Add(baseDamageResistancesType[i], baseDamageResistancesModifier[i]);
        }

        currentDamageResistances = new Dictionary<E_DamageTypes, float>();

        foreach (E_DamageTypes type in baseDamageResistances.Keys)
        {
            currentDamageResistances.Add(type, baseDamageResistances[type]);
        }
    }

    public int ChangeHealth(E_DamageTypes type, int value)
    {
        //Resistance check
        int trueValue = (int)(value * CheckResistances(type));

        if (type == E_DamageTypes.Healing)
        {
            health = Mathf.Clamp(trueValue + trueValue, 0, maxHealth);
        }
        else if (type == E_DamageTypes.Shield)
        {
            shield += trueValue;
        }
        else if (type == E_DamageTypes.Arcana)
        {
            //Increase arcana
        }
        else
        {
            int damageOverShield = (int)Mathf.Clamp(trueValue - shield, 0, Mathf.Infinity);
            shield = (int)Mathf.Clamp(shield - trueValue, 0, Mathf.Infinity);
            health -= damageOverShield;
        }

        return trueValue;
    }

    public bool ModifyResistanceModifier(E_DamageTypes damageType, float newValue)
    {
        if (currentDamageResistances.ContainsKey(damageType))
        {
            currentDamageResistances[damageType] += newValue;
            return true;
        }
        else
        {
            currentDamageResistances.Add(damageType, newValue);
            return false;
        }
    }

    public float CheckResistances(E_DamageTypes type)
    {
        //needs to check resistances
        if (currentDamageResistances.ContainsKey(type))
        {
            return currentDamageResistances[type];
        }
        else
        {
            //No resistance modifier, return 1
            return 1;
        }
    }
}