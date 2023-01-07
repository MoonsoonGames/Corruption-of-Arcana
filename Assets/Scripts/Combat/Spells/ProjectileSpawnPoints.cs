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
    public class ProjectileSpawnPoints : MonoBehaviour
    {
        public CombatHelperFunctions.ProjectilePoint[] playerPoints;
        public CombatHelperFunctions.ProjectilePoint[] enemyPoints;

        public Vector2 GetPointPos(E_ProjectilePoints pointEnum, bool playerTeam)
        {
            Vector2 pos = new Vector2(0, 0);

            if (playerTeam)
            {
                foreach (var item in playerPoints)
                {
                    if (item.point == pointEnum)
                    {
                        pos.x = item.transform.position.x;
                        pos.y = item.transform.position.y;
                    }
                }
            }
            else
            {
                foreach (var item in enemyPoints)
                {
                    if (item.point == pointEnum)
                    {
                        pos.x = item.transform.position.x;
                        pos.y = item.transform.position.y;
                    }
                }
            }

            return pos;
        }
    }
}
