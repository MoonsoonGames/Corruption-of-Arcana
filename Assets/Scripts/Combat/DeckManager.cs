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
    public class DeckManager : MonoBehaviour
    {
        public List<Spell> spells;
        public List<Spell> discard;

        // Start is called before the first frame update
        public Spell GetSpell()
        {
            if (spells.Count == 0)
            {
                DrawFromDiscard();
            }

            if (spells.Count != 0)
            {
                Spell spell = spells[Random.Range(0, spells.Count)];

                spells.Remove(spell);

                return spell;
            }

            return null;
        }

        private void Start()
        {
            spells.Sort(ShuffleDeck);
        }

        public void ReturnCard(Spell spell)
        {
            discard.Add(spell);
        }

        void DrawFromDiscard()
        {
            discard.Sort(ShuffleDeck);

            foreach (Spell spell in discard)
            {
                spells.Add(spell);
            }

            discard.Clear();
        }

        static int ShuffleDeck(Spell s1, Spell s2)
        {
            float random1 = Random.Range(0f, 1f);
            float random2 = Random.Range(0f, 1f);

            return random1.CompareTo(random2);
        }
    }
}