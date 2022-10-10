using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Timeline timeline;

    public List<Enemy> enemies = new List<Enemy>();
    public Character player;

    private void Start()
    {
        timeline = GameObject.FindObjectOfType<Timeline>();
    }

    public void Add(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    public void Remove(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void StartTurn()
    {
        foreach (Enemy enemy in enemies)
        {
            //In future, determine target depending on spell so it can cast support spells on allies/self
            SpellInstance newSpellInstance = new SpellInstance();
            newSpellInstance.SetSpellInstance(enemy.PrepareSpell(), player, enemy);

            timeline.AddCard(newSpellInstance);
        }
    }
}
