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
    public class ProjectileMovement : MonoBehaviour
    {
        public float distanceAllowance = 0.1f;

        public Object impactEffect;
        Image image;

        bool moving = false;
        List<Vector2> movePositions;
        int currentTarget = 0;
        float speed = 0;

        public void Setup(Color color, Object effect)
        {
            image = GetComponent<Image>();
            image.color = color;
            impactEffect = effect;
        }

        public void MoveToPositions(float newSpeed, List<Vector2> newMovePositions)
        {
            movePositions = newMovePositions;
            speed = newSpeed;
            moving = true;
        }

        private void FixedUpdate()
        {
            if (moving && movePositions.Count != 0)
            {
                if (movePositions.Count > currentTarget)
                {
                    float lerpX = Mathf.Lerp(transform.position.x, movePositions[currentTarget].x, speed);
                    float lerpY = Mathf.Lerp(transform.position.y, movePositions[currentTarget].y, speed);

                    Vector2 newPos = new Vector2(lerpX, lerpY);

                    transform.position = newPos;

                    if (Vector2.Distance(newPos, movePositions[currentTarget]) < distanceAllowance)
                    {
                        currentTarget++;
                    }
                }
                else
                {
                    Impact();
                }
            }
        }

        void Impact()
        {
            Destroy(this.gameObject);
        }
    }
}
