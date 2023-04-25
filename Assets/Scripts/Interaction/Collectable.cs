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
            Setup();
        }

        [ContextMenu("Setup")]
        private void Setup()
        {
            sprite.sprite = spell.cardImage;
        }

        public void SetID(string newID)
        {
            ID = newID;
        }

        public void Interacted(GameObject player)
        {
            if (spell != null)
            {
                if (spell.potionCost > 0)
                {
                    PotionManager.instance.ChangePotion(spell.potionType, 1);
                }
                else
                {
                    DeckManager.instance.collection.Add(spell);
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
