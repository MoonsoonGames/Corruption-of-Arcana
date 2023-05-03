using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    public class SetupVFX : MonoBehaviour
    {
        public Object testPoints;

        public void Setup(Vector3 casterPos, Vector3 targetPos)
        {
            Debug.Log("Visual effect setup");
            if (testPoints != null)
            {
                Instantiate(testPoints, casterPos, new Quaternion(0, 0, 0, 0));
                Instantiate(testPoints, targetPos, new Quaternion(0, 0, 0, 0));
            }

            VisualEffect visualEffect = GetComponentInChildren<VisualEffect>();

            visualEffect.SetVector3("Impact Position", targetPos);

            float distance = Vector3.Distance(casterPos, targetPos)/2.35f;

            visualEffect.SetFloat("Beam end", distance);
            
            Vector3 add = new Vector3(300, 300, 0);

            casterPos += add;
            targetPos += add;

            casterPos.z = 0;
            targetPos.z = 0;

            Vector3 rotation = Quaternion.FromToRotation(casterPos, targetPos).eulerAngles;
            //Vector3 rotation = targetPos - casterPos;
            rotation.z += 45f;
            visualEffect.SetVector3("Beam rotation", rotation);
        }
    }
}
