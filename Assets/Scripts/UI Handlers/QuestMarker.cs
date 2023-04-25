using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class QuestMarker : MonoBehaviour
    {
        public Sprite icon;
        public Image image;

        public Vector2 position
        {
            get 
            {
                return new Vector2 (transform.position.x, transform.position.y);
            }
        }
    }
}
