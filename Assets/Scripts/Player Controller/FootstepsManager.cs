using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/// <summary>
/// Authored & Written by <NAME/TAG/SOCIAL LINK>
/// 
/// Use by NPS is allowed as a collective, for external use, please contact me directly
/// </summary>
namespace Necropanda
{
    /// <summary>
    /// This controls footstep audio on multiple terrain types.
    /// </summary>
    public class FootstepsManager : MonoBehaviour
    {
        private enum CURRENT_TERRAIN { Grass, Dirt,Stone, Wood,Crystal, Water }

        [SerializeField]
        private CURRENT_TERRAIN currentTerrain;

        private FMOD.Studio.EventInstance footsteps;

        private FootstepsFX footstepsFX;

        private void Start()
        {
            footstepsFX = GetComponent<FootstepsFX>();
        }

        private void Update() {
            DetermineTerrainType();
        }

        /// <summary>
        /// This determines what type of terrain the player is on. 
        /// There might be a more performant way to do this using groundsphere
        /// checking.
        /// </summary>
        private void DetermineTerrainType()
        {
            RaycastHit[] hit;

            // Send a ray from the players postion down 10 units.
            hit = Physics.RaycastAll(transform.position, Vector3.down, 10f);

            foreach (RaycastHit rayHit in hit)
            {
                // Can probably be converted to a switch statement, I hate how this reads...
                if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
                {
                    currentTerrain = CURRENT_TERRAIN.Dirt;
                }
                else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
                {
                    currentTerrain = CURRENT_TERRAIN.Wood;
                }
                else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
                {
                    currentTerrain = CURRENT_TERRAIN.Grass;
                }
                else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    currentTerrain = CURRENT_TERRAIN.Water;
                }
                else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Crystal"))
                {
                    currentTerrain = CURRENT_TERRAIN.Crystal;
                }
                else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Stone"))
                {
                    currentTerrain = CURRENT_TERRAIN.Stone;
                }
            }
        }

        /// <summary>
        /// This function sets up FMOD and plays the audio based on the passed in value.
        /// </summary>
        /// <param name="terrainType">The type of terrain the player is one. 
        /// Each type has a number in FMOD</param>
        private void PlayFootstep (int terrainType, Vector3 pos)
        {
            footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Footsteps");
            footsteps.setParameterByName("Terrain", terrainType);
            footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            footsteps.start();
            footsteps.release();

            footstepsFX.SpawnFootstepFX(terrainType, pos);
        }

        public void SelectAndPlayFootstep(float xOffset)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
            {
                Vector3 pos = hit.point;

                pos.x += xOffset;


                switch (currentTerrain)
                {
                    case CURRENT_TERRAIN.Grass:
                        PlayFootstep(1, pos);
                        break;

                    case CURRENT_TERRAIN.Stone:
                        PlayFootstep(0, pos);
                        break;

                    case CURRENT_TERRAIN.Dirt:
                        PlayFootstep(2, pos);
                        break;

                    case CURRENT_TERRAIN.Wood:
                        PlayFootstep(3, pos);
                        break;

                    case CURRENT_TERRAIN.Water:
                        PlayFootstep(4, pos);
                        break;

                    case CURRENT_TERRAIN.Crystal:
                        PlayFootstep(5, pos);
                        break;

                    default:
                        Debug.LogWarning("No valid terrain type was found, reverting to default case");
                        PlayFootstep(0, pos);
                        break;
                }
            }
            
        }
    }
}
