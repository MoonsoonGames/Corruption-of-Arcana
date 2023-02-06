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
    public class Collectable : MonoBehaviour, IInteractable
    {
        public Spell spell;
        public SpriteRenderer sprite;
        public string ID;

        private void Start()
        {
            sprite.sprite = spell.cardImage;
        }

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject player)
        {
            DeckManager.instance.collection.Add(spell);
            Destroy(this.gameObject);
        }
    }
}
