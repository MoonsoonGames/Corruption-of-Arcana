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

            Vector2 randMove = new Vector2();
            randMove.x = Random.Range(-intensity, intensity);
            randMove.y = Random.Range(-intensity, intensity);

            Vector3 newPos = new Vector3(randMove.x, randMove.y, 0);

            Debug.Log(name + " shakes from " + transform.position + " to " + newPos);
            transform.position += newPos;

            if (gameObject.activeSelf)
                StartCoroutine(ResetShake(randMove, duration));
        }

        IEnumerator ResetShake(Vector2 offset, float delay)
        {
            yield return new WaitForSeconds(delay);
            Vector3 newPos = new Vector3(-offset.x, -offset.y, 0);
            Debug.Log(name + " returns from " + transform.position + " to " + newPos);
            transform.position += newPos;
        }
    }
}
