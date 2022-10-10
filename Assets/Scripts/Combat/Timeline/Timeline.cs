using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    List<SpellInstance> spells = new List<SpellInstance>();
    List<SpellBlock> spellBlocks = new List<SpellBlock>();
    public Object spellBlockPrefab;

    public void AddCard(SpellInstance newSpellInstance)
    {
        spells.Add(newSpellInstance);
        SortCards();
        CalculateTimeline();
    }

    public void RemoveCard(SpellInstance newSpellInstance)
    {
        spells.Remove(newSpellInstance);
        SortCards();
        CalculateTimeline();
    }

    void CalculateTimeline()
    {
        //Clear old blocks
        foreach (var item in spellBlocks)
        {
            Destroy(item.gameObject);
        }

        spellBlocks.Clear();

        //Spawn UI for cards
        foreach (var item in spells)
        {
            string text = item.caster.characterName + " is casting " + item.spell.spellName + " on " + item.target.characterName + " (" + item.spell.speed + ")";

            GameObject spellBlockObject = Instantiate(spellBlockPrefab) as GameObject;
            spellBlockObject.transform.SetParent(transform, false);

            SpellBlock spellBlock = spellBlockObject.GetComponent<SpellBlock>();
            spellBlock.text.text = text;
            spellBlock.image.color = item.spell.timelineColor;
            spellBlocks.Add(spellBlock);
        }
    }

    public void SortCards()
    {
        List<SpellInstance> orderedList = new List<SpellInstance>();

        foreach (var item in spells)
        {
            SpellInstance newSpellInstance = new SpellInstance();
            newSpellInstance.SetSpellInstance(item.spell, item.target, item.caster);

            orderedList.Add(newSpellInstance);
        }

        orderedList.Sort(SortBySpeed);

        spells.Clear();

        foreach (var item in orderedList)
        {
            SpellInstance newSpellInstance = new SpellInstance();
            newSpellInstance.SetSpellInstance(item.spell, item.target, item.caster);

            spells.Add(newSpellInstance);
            //Debug.Log(item.Key.spellName + item.Key.speed);
        }
    }

    static int SortBySpeed(SpellInstance c1, SpellInstance c2)
    {
        return c1.spell.speed.CompareTo(c2.spell.speed);
    }

    public float CastSpells()
    {
        Debug.Log("Casting spells");
        float delay = 0;

        if (spells.Count > 0)
        {
            delay = spells[spells.Count - 1].spell.speed;
        }

        //Loop through list and cast spell;
        foreach (var item in spells)
        {
            SpellInstance newSpellInstance = new SpellInstance();
            newSpellInstance.SetSpellInstance(item.spell, item.target, item.caster);

            StartCoroutine(IDelaySpell(newSpellInstance));
        }

        return delay;
    }

    IEnumerator IDelaySpell(SpellInstance spellInstance)
    {
        yield return new WaitForSeconds(spellInstance.spell.speed);

        Debug.Log(spellInstance.caster.characterName + " played " + spellInstance.spell.spellName + " on " + spellInstance.target.characterName + " at time " + spellInstance.spell.speed);

        spellInstance.spell.CastSpell(spellInstance.target, spellInstance.caster);

        RemoveCard(spellInstance);
        CalculateTimeline();
    }
}