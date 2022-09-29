using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Health Values
    public int maxHealth;
    protected int health;

    //Damage Resistances
    public Dictionary<E_DamageTypes, float> baseDamageResistances;
    Dictionary<E_DamageTypes, float> currentDamageResistances;

    protected virtual void SetupHealth()
    {
        health = maxHealth;
    }

    protected virtual void SetupResistances()
    {
        currentDamageResistances = new Dictionary<E_DamageTypes, float>();

        foreach (E_DamageTypes type in baseDamageResistances.Keys)
        {
            currentDamageResistances.Add(type, baseDamageResistances[type]);
        }
    }

    public int TakeDamage(int damage)
    {
        int trueDamage = damage;

        //Resistance check

        health -= damage;
        return damage;
    }

    public int Heal(int healthRestore)
    {
        health -= healthRestore;
        return healthRestore;
    }

    public bool AddResistanceModifier(E_DamageTypes damageType, float newValue)
    {
        if (currentDamageResistances.ContainsKey(damageType))
        {
            currentDamageResistances[damageType] = newValue;
            return true;
        }
        else
        {
            currentDamageResistances.Add(damageType, newValue);
            return false;
        }
    }

    public int CheckResistances(int damage, E_DamageTypes damageType)
    {
        //needs to check resistances
        return damage;
    }
}