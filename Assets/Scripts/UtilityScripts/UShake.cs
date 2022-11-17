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
    public class UShake : MonoBehaviour
    {
        public float baseDuration = 0.2f;
        public float intensityMultiplier = 0.005f;

        public void CharacterShake(float duration, float intensity)
        {
            Debug.Log("Character shake");
            Vector3 originalPos = gameObject.transform.position;

            float randx = transform.position.x + Random.Range(-intensity, intensity);
            float randy = transform.position.y + Random.Range(-intensity, intensity);

            Vector3 newPos = new Vector3(randx, randy, transform.position.z);

            //Debug.Log(name + " shakes from " + transform.position + " to " + newPos);
            transform.position = newPos;

            StartCoroutine(ResetShake(originalPos, duration));
        }

        IEnumerator ResetShake(Vector3 originalPosition, float delay)
        {
            yield return new WaitForSeconds(delay);
            //Debug.Log(name + " returns from " + transform.position + " to " + originalPosition);
            transform.position = originalPosition;
        }
    }
}
