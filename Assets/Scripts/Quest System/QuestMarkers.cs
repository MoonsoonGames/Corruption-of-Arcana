using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class QuestMarkers : RequireQuestProgress
    {
        Compass compass;
        SpriteRenderer sprite;
        QuestMarker questMarker;

        protected override void Start()
        {
            //Do nothing
        }

        public void Setup()
        {
            sprite = GetComponent<SpriteRenderer>();
            questMarker = GetComponent<QuestMarker>();
            questMarker.icon = sprite.sprite;

            compass = GameObject.FindObjectOfType<Compass>();
        }

        protected override void SetMarker(bool active)
        {
            //gameObject.SetActive(active);
            sprite.color = active ? new Color(255, 255, 255, 255) : new Color(0, 0, 0, 0);

            if (compass != null)
            {
                if (active)
                    compass.AddQuestMarker(questMarker);
                else
                    compass.RemoveQuestMarker(questMarker);
            }
        }
    }
}
