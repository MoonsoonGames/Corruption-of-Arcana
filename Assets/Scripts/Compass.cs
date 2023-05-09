using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class Compass : MonoBehaviour
    {
        public float offset = 22.5f;
        public RawImage compassHeadings;
        public Image MarkerHolder;
        public Transform player;
        public float MaxDistance = 300f; 

        public GameObject iconPrefab;
        List<QuestMarker> questMarkers = new List<QuestMarker>();

        float compassUnit;

        QuestMarkers[] markers;

        private void Start()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
            }

            compassUnit = MarkerHolder.rectTransform.rect.width / 360f;

            markers = GameObject.FindObjectsOfType<QuestMarkers>(true);
            CheckQuestMarkers();
        }

        public void Update()
        {
            compassHeadings.uvRect = new Rect((player.localEulerAngles.y / 360f) + offset, 1f, 1f, 1f);

            foreach(QuestMarker marker in questMarkers)
            {
                marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);

                float dst = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), marker.position);
                float scale = .25f;

                if (dst < MaxDistance)
                {
                    scale = 1f - (dst / MaxDistance);
                }
                marker.image.rectTransform.localScale = Vector3.one * scale;
            }
        }

        public void AddQuestMarker(QuestMarker marker)
        {
            if (questMarkers.Contains(marker))
                return;
            GameObject newMarker = Instantiate(iconPrefab, MarkerHolder.transform);
            marker.image = newMarker.GetComponent<Image>();
            marker.image.sprite = marker.icon;

            questMarkers.Add(marker);
        }

        public void RemoveQuestMarker(QuestMarker marker)
        {
            if (!questMarkers.Contains(marker))
                return;
            questMarkers.Remove(marker);
            Destroy(marker.image);
        }

        Vector2 GetPosOnCompass (QuestMarker marker)
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
            Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);

            float angle = Vector2.SignedAngle (marker.position - playerPos, playerFwd);

            return new Vector2(compassUnit * angle, 0f);
        }

        public void CheckQuestMarkers()
        {
            foreach (var item in markers)
            {
                item.Setup();
                item.CheckProgress();
            }
        }
    }
}