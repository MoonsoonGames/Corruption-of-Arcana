using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Combat/Spells", order = 0)]
public class Spell : ScriptableObject
{
    public string spellName;
    public Color timelineColor;
    [TextArea(3, 10)]
    public string spellDescription; // Basic desciption of spell effect

    public E_DamageTypes effectTypeCaster;
    public int valueCaster;
    //Caster status effects here

    public E_DamageTypes effectTypeTarget;
    public int valueTarget;
    public int hitCount = 1;
    public E_MultihitType multihitType;
    public int multihitValue;
    //Target status effects here

    public float speed;
    public int arcanaCost;

    public void CastSpell(Character target, Character caster, EnemyManager enemyManager)
    {
        if (target != null)
        {
            for (int i = 0; i < hitCount; i++)
            {
                target.GetHealth().ChangeHealth(effectTypeTarget, valueTarget);

                if (multihitType != E_MultihitType.Single)
                {
                    List<Character> targets = enemyManager.enemies;

                    switch (multihitType)
                    {
                        case (E_MultihitType.Chain):

                            break;
                        case (E_MultihitType.Cleave):
                            break;

                    }
                }
            }
        }
    }
}