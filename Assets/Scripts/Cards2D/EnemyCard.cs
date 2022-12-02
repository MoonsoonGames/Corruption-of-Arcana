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
    public class EnemyCard : MonoBehaviour
    {
        Card card;

        public void Setup(Spell spell)
        {
            card = GetComponent<Card>();
            card.spell = spell;
            card.Setup();
        }
    }
}
