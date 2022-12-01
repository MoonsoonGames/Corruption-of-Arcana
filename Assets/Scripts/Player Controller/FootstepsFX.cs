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
    public class FootstepsFX : MonoBehaviour
    {
        public Object grass, dirt, stone, wood, crystal, water;

        public void SpawnFootstepFX(int terrain, Vector3 pos)
        {
            Debug.Log("Spawning effect");
            switch (terrain)
            {
                case 0:
                    Instantiate(grass, pos, new Quaternion(0, 0, 0, 0));
                    break;
                case 1:
                    Instantiate(dirt, pos, new Quaternion(0, 0, 0, 0));
                    break;
                case 2:
                    Instantiate(stone, pos, new Quaternion(0, 0, 0, 0));
                    break;
                case 3:
                    Instantiate(wood, pos, new Quaternion(0, 0, 0, 0));
                    break;
                case 4:
                    Instantiate(crystal, pos, new Quaternion(0, 0, 0, 0));
                    break;
                case 5:
                    Instantiate(water, pos, new Quaternion(0, 0, 0, 0));
                    break;
                default:
                    Debug.Log("Not Spawning effect");
                    break;
            }
        }
    }
}