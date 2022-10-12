using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    EnemyManager enemyManager;

    public List<Spell> spells = new List<Spell>();

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<CharacterHealth>();

        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        enemyManager.Add(this);
    }

    public override void Die()
    {
        //Clear self from enemy manager;
        enemyManager.Remove(this);

        base.Die();
    }

    public Spell PrepareSpell()
    {
        return spells[Random.Range(0, spells.Count)];
    }
}