using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Authored & Written by Andrew Scott andrewscott@icloud.com
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class ProjectileMovement : MonoBehaviour
    {
        public float distanceAllowance = 0.5f;

        public Object impactEffect;
        Image[] images;
        TrailRenderer[] trailRenderers;

        bool moving = false;
        Vector2[] movePositions;
        int currentTarget = 0;
        float speed = 0;

        public void Setup(Color color, Object impactEffect, Object projectileEffect)
        {
            images = GetComponentsInChildren<Image>();
            trailRenderers = GetComponentsInChildren<TrailRenderer>();

            foreach (var item in images)
            {
                if (projectileEffect != null)
                {
                    item.color = new Color(color.r, color.g, color.b, 0);

                    Instantiate(projectileEffect, item.gameObject.transform);
                }
                else
                    item.color = color;
            }

            foreach (var item in trailRenderers)
            {
                item.startColor = color;
            }

            this.impactEffect = impactEffect;
        }

        public void MoveToPositions(float newSpeed, Vector2[] newMovePositions)
        {
            movePositions = newMovePositions;
            speed = newSpeed;
            moving = true;
        }

        private void FixedUpdate()
        {
            if (moving && movePositions.Length != 0)
            {
                if (movePositions.Length > currentTarget)
                {
                    //Vector3.SmoothDamp() is a cool thing
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
            if (impactEffect != null)
            {
                Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                spawnPos.z = VFXManager.instance.transform.position.z;
                VFXManager.instance.SpawnImpact(impactEffect, spawnPos);
            }

            Destroy(this.gameObject);
        }
    }
}
