using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCardValues : MonoBehaviour
{
    public Spell[] spells;

    // Start is called before the first frame update
    void Start()
    {
        Spell spell = spells[Random.Range(0, spells.Length)];

        Card card = GetComponent<Card>();
        card.spell = spell;
        card.Setup();
    }
}
