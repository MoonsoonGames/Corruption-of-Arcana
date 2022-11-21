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
    public class WaterLevel : MonoBehaviour
    {
        public List<float> waterLevels;
        public float scaleSpeed = 0.005f;
        public float accuracyThreshold = 1f;

        bool moving = false;
        Vector3 desiredPos;

        public void AdjustWaterLevel()
        {
            if (waterLevels.Count <= 0)
            {
                //There are no water levels set
                Debug.LogWarning("No water levels have been set");
                return;
            }

            //Dequeue and enqueue water level
            float newY = waterLevels[0];
            waterLevels.RemoveAt(0);
            waterLevels.Add(newY);

            //Set new y value of the water to the dequeued value
            desiredPos = gameObject.transform.localPosition;
            desiredPos.y = newY;
        }

        private void Update()
        {
            if (HelperFunctions.AlmostEqual(transform.localPosition.y, desiredPos.y, accuracyThreshold) == false)
            {
                float posX = transform.localPosition.x;
                float lerpY = Mathf.Lerp(transform.localPosition.y, desiredPos.y, scaleSpeed * Time.deltaTime);
                float posZ = transform.localPosition.z;

                transform.localPosition = new Vector3(posX, lerpY, posZ);
            }
        }
    }
}
