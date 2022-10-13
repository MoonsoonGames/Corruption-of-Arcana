using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
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

}