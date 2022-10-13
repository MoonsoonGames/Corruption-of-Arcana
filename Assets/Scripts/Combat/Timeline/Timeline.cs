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
    public class Timeline : MonoBehaviour
    {
        #region Setup

        List<SpellInstance> spells = new List<SpellInstance>();
        List<SpellBlock> spellBlocks = new List<SpellBlock>();
        public Object spellBlockPrefab;

        public Character player;
        ArcanaManager arcanaManager;
        int arcanaCount = 0;

        private void Start()
        {
            arcanaManager = player.GetComponent<ArcanaManager>();
        }

        #endregion

        #region Changing Timeline

        #region Adding/Removing Spell Instances

        /// <summary>
        /// Adds spell instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void AddSpellInstance(SpellInstance newSpellInstance)
        {
            spells.Add(newSpellInstance);
            CalculateTimeline();
        }

        /// <summary>
        /// Removes spell instance to the timeline
        /// </summary>
        /// <param name="newSpellInstance"></param>
        public void RemoveSpellInstance(SpellInstance newSpellInstance)
        {
            spells.Remove(newSpellInstance);
            CalculateTimeline();
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Sorts the list of spells by their speed and spawns UI blocks on the timeline
        /// </summary>
        void CalculateTimeline()
        {
            spells.Sort(SortBySpeed);
            arcanaCount = 0;

            //Clear old blocks that are no longer being cast
            foreach (var item in spellBlocks)
            {
                Destroy(item.gameObject);
            }

            spellBlocks.Clear();

            //Spawn UI for cards
            foreach (var item in spells)
            {
                string text = item.caster.characterName + " is casting " + item.spell.spellName + " on " + item.target.characterName + " (" + item.spell.speed + ")";

                //Creates spell block game object
                GameObject spellBlockObject = Instantiate(spellBlockPrefab) as GameObject;
                spellBlockObject.transform.SetParent(transform, false);

                //Sets spell block values
                SpellBlock spellBlock = spellBlockObject.GetComponent<SpellBlock>();
                spellBlock.text.text = text;
                if (item.spell.overrideColor)
                    spellBlock.image.color = item.spell.timelineColor;
                else
                    spellBlock.image.color = item.caster.timelineColor;

                //Adds spell block to layout group
                spellBlocks.Add(spellBlock);

                if (item.caster == player)
                {
                    arcanaCount += item.spell.arcanaCost;
                }
            }

            arcanaManager.CheckArcana(arcanaCount);
        }

        static int SortBySpeed(SpellInstance c1, SpellInstance c2)
        {
            return c1.spell.speed.CompareTo(c2.spell.speed);
        }

        #endregion

        #endregion

        #region Spellcasting

        /// <summary>
        /// Casts every spell on the timeline and then removes them
        /// </summary>
        /// <returns></returns>
        public float CastSpells()
        {
            //Generates a delay for the entire set of spells being cast
            float delay = 0;

            if (spells.Count > 0)
            {
                delay = spells[spells.Count - 1].spell.speed;
            }

            //Loop through list and cast spell;
            foreach (var item in spells)
            {
                //Use a coroutine to stagger spellcasting
                StartCoroutine(IDelaySpell(item));
            }

            return delay;
        }

        /// <summary>
        /// Delays the casting of a spell by its speed
        /// </summary>
        /// <param name="spellInstance"></param>
        /// <returns></returns>
        IEnumerator IDelaySpell(SpellInstance spellInstance)
        {
            yield return new WaitForSeconds(spellInstance.spell.speed);

            Debug.Log(spellInstance.caster.characterName + " played " + spellInstance.spell.spellName + " on " + spellInstance.target.characterName + " at time " + spellInstance.spell.speed);

            spellInstance.spell.CastSpell(spellInstance.target, spellInstance.caster);

            RemoveSpellInstance(spellInstance);
            CalculateTimeline();
        }

        #endregion
    }
}