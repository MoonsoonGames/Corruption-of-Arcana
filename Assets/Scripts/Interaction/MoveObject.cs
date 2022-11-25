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
    public class MoveObject : MonoBehaviour
    {
        public List<Vector3> positions;
        public float scaleSpeed = 0.7f;
        public float accuracyThreshold = 0.2f;

        bool moving = false;
        Vector3 desiredPos;

        public void AdjustPosition()
        {
            if (positions.Count <= 0)
            {
                //There are positions set
                Debug.LogWarning("No positions have been set");
                return;
            }

            //Set desired position of the object to the first value
            desiredPos = positions[0];

            //Dequeue and enqueue desired pos
            positions.RemoveAt(0);
            positions.Add(desiredPos);

            SoundFX(true);
            moving = true;
        }

        private void Update()
        {
            if (moving)
            {
                if (HelperFunctions.AlmostEqualVector3(transform.localPosition, desiredPos, accuracyThreshold) == false)
                {
                    float lerpX = Mathf.Lerp(transform.localPosition.x, desiredPos.x, scaleSpeed * Time.deltaTime);
                    float lerpY = Mathf.Lerp(transform.localPosition.y, desiredPos.y, scaleSpeed * Time.deltaTime);
                    float lerpZ = Mathf.Lerp(transform.localPosition.z, desiredPos.z, scaleSpeed * Time.deltaTime);

                    transform.localPosition = new Vector3(lerpX, lerpY, lerpZ);
                }
                else
                {
                    moving = false;
                    SoundFX(false);
                }
            }
        }

        public void SoundFX(bool start)
        {
            if (start)
            {
                //TODO: Start playing water moving sound
                Debug.Log("TODO: Start playing moving sound");
            }
            else
            {
                //TODO: Stop playing water moving sound
                Debug.Log("TODO: Stop playing moving sound");
            }
        }
    }
}
