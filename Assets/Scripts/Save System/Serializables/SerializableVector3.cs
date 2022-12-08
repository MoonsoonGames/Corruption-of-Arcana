using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Authored & Written by @mattordev
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda.SaveSystem.Serializables
{
    [System.Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        //whenever you want to create a serializable vector out of a Vector you do "new SerializableVector3 (yourVector);"
        public SerializableVector3(Vector3 yourVector)
        {
            x = yourVector.x;
            y = yourVector.y;
            z = yourVector.z;
        }
    }
}
