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
    public class VFXManager : MonoBehaviour
    {
        #region Setup

        public static VFXManager instance;
        public Vector2 middlePositionOffset;
        public float speedCalculationMultiplier = 700;
        ProjectileSpawnPoints projectileSpawner;

        private void Start()
        {
            instance = this;

            projectileSpawner = GetComponentInChildren<ProjectileSpawnPoints>();
        }

        #endregion

        #region Spell Logic

        public void AffectSelfDelay(Spell spellRef, Character caster, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, float delay, bool empowered, bool weakened)
        {
            StartCoroutine(IDelayAffectSelf(spellRef, caster, spell, effectType, cardsDiscarded, removedStatuses, delay, empowered, weakened));
        }

        IEnumerator IDelayAffectSelf(Spell spellRef, Character caster, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, float delay, bool empowered, bool weakened)
        {
            yield return new WaitForSeconds(delay);
            Vector2[] points = GetProjectilePoints(spellRef.projectilePoints, caster, caster);
            float effectDelay = QueryTime(points, spellRef.projectileSpeed);

            VFXManager.instance.SpawnProjectile(points, spellRef.projectileObject, spellRef.projectileSpeed, spellRef.trailColor, spellRef.impactObject, effectType);
            SpawnCastEffect(points, spellRef.castObject);
            yield return new WaitForSeconds(effectDelay);
            spellRef.AffectSelf(caster, spell, effectType, cardsDiscarded, removedStatuses, empowered, weakened);
        }

        public void AffectTargetDelay(Spell spellRef, Character caster, Character target, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, float delay, bool empowered, bool weakened)
        {
            StartCoroutine(IDelayAffectTarget(spellRef, caster, target, spell, effectType, cardsDiscarded, removedStatuses, delay, empowered, weakened));
        }

        IEnumerator IDelayAffectTarget(Spell spellRef, Character caster, Character target, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, float delay, bool empowered, bool weakened)
        {
            yield return new WaitForSeconds(delay);
            Vector2[] points = GetProjectilePoints(spellRef.projectilePoints, caster, target);
            float effectDelay = QueryTime(points, spellRef.projectileSpeed);

            VFXManager.instance.SpawnProjectile(points, spellRef.projectileObject, spellRef.projectileSpeed, spellRef.trailColor, spellRef.impactObject, effectType);
            SpawnCastEffect(points, spellRef.castObject);
            yield return new WaitForSeconds(effectDelay);
            spellRef.AffectTarget(caster, target, spell, effectType, cardsDiscarded, removedStatuses, empowered, weakened);
        }

        public float QueryTime(Vector2[] points, float projectileSpeed)
        {
            //Calculate and return delay (T=D/S)
            float distance = 0;

            for (int i = 0; i < points.Length - 1; i++)
            {
                distance += Vector2.Distance(points[i], points[i + 1]);
            }

            float time = distance / (projectileSpeed * speedCalculationMultiplier);
            //Debug.Log(time);
            time += Time.fixedDeltaTime / projectileSpeed; //Instead of 1, use the time between frames
            //float fixedFrameTime = Time.fixedDeltaTime;
            return time;
        }

        #endregion

        #region VFX

        public void SpawnCastEffect(Vector2[] points, Object effectRef)
        {
            if (effectRef == null)
            {
                Debug.Log("No reference to projectile");
                return;
            }

            //Spawn projectile at spawn position
            GameObject effectObject;
            effectObject = Instantiate(effectRef, this.gameObject.transform) as GameObject;

            if (effectObject == null)
            {
                Debug.Log("No game object spawned, returning");
                return;
            }

            
            effectObject.transform.position = points[0];
            Vector2 direction = points[points.Length-1] - points[0];
            effectObject.transform.rotation = Quaternion.LookRotation(direction);

            Debug.Log("Game object spawned at " + effectObject.transform.position);
        }

        /// <summary>
        /// Spawn a projectile at a specified position, which will then move towards each point in 
        /// </summary>
        /// <param name="spawnPosition">Initial position for the projectile to be spawned at</param>
        /// <param name="targetPositions">All of the points the projectile will move to</param>
        /// <param name="projectileRef">Object reference of the projectile</param>
        /// <param name="trailColor">Colour of the projectile and trail, leave as (0, 0, 0, 0) to default to the damage type</param>
        /// <param name="impactRef">Impact effect of the projectile and trail, leave as null to default to the damage type</param>
        /// <param name="damageType">Damage type dealt by the projectile</param>
        public void SpawnProjectile(Vector2[] points, Object projectileRef, float projectileSpeed, Color trailColor, Object impactRef, E_DamageTypes damageType)
        {
            if (points.Length <= 0) { return; }

            #region Spawning the projectile as an game object

            if (projectileRef == null)
            {
                Debug.Log("No reference to projectile");
                return;
            }

            //Spawn projectile at spawn position
            GameObject projectileObject;
            projectileObject = Instantiate(projectileRef, this.gameObject.transform) as GameObject;

            if (projectileObject == null)
            {
                Debug.Log("No game object spawned, returning");
                return;
            }

            #region Get Color and Impact

            Object impactFX;
            if (impactRef != null)
            {
                impactFX = impactRef;
            }
            else
            {
                impactFX = ImpactObjectFromDamageType(damageType);
            }

            Color color;
            Color testColor = new Color(0, 0, 0, 0);
            if (trailColor != testColor)
            {
                color = trailColor;
            }
            else
            {
                color = ColourFromDamageType(damageType);
            }

            #endregion

            #endregion

            #region Setting up the projectile movement

            projectileObject.transform.position = points[0];

            ProjectileMovement projectileMovement = projectileObject.GetComponent<ProjectileMovement>();

            projectileMovement.Setup(color, impactFX);

            projectileMovement.MoveToPositions(projectileSpeed, points);

            #endregion
        }

        /// <summary>
        /// Spawns an impact effect at a specified position
        /// </summary>
        /// <param name="impactEffect">An object reference to the particle effect</param>
        /// <param name="spawnPos">The spawn position</param>
        public void SpawnImpact(Object impactEffect, Vector3 spawnPos)
        {
            if (impactEffect != null)
            {
                GameObject impactRef;
                impactRef = Instantiate(impactEffect, transform) as GameObject;

                if (impactRef != null)
                {
                    impactRef.transform.position = spawnPos;
                }
                else
                {
                    Debug.LogWarning("No reference");
                }
            }
        }

        public Vector2[] GetProjectilePoints(E_ProjectilePoints[] projectileEnums, Character caster, Character target)
        {
            Vector2[] projectilePoints = new Vector2[(int)projectileEnums.Length];
            bool playerTeam = caster.GetManager() == CombatManager.instance.playerTeamManager;
            for (int i = 0; i < projectilePoints.Length; i++)
            {
                switch (projectileEnums[i])
                {
                    case E_ProjectilePoints.Caster:
                        if (caster != null)
                            projectilePoints[i] = caster.transform.position;
                        break;
                    case E_ProjectilePoints.Target:
                        if (target != null)
                            projectilePoints[i] = target.transform.position;
                        break;
                    case E_ProjectilePoints.TimeBlock:
                        //TODO: Not implemented Properly
                        projectilePoints[i] = projectileSpawner.GetPointPos(projectileEnums[i], playerTeam);
                        break;
                    default:
                        projectilePoints[i] = projectileSpawner.GetPointPos(projectileEnums[i], playerTeam);
                        break;
                }
            }

            return projectilePoints;
        }

        #region Colour

        [Header("Colour based on damage")]
        public Color physicalColour;
        public Color perforationColour;
        public Color septicColour;
        public Color bleakColour;
        public Color staticColour;
        public Color emberColour;

        public Color healColour;
        public Color shieldColour;
        public Color arcanaColour;

        public Color defaultColour;

        Color ColourFromDamageType(E_DamageTypes damageType)
        {
            switch (damageType)
            {
                case E_DamageTypes.Physical:
                    return physicalColour;
                case E_DamageTypes.Perforation:
                    return perforationColour;
                case E_DamageTypes.Septic:
                    return septicColour;
                case E_DamageTypes.Bleak:
                    return bleakColour;
                case E_DamageTypes.Static:
                    return staticColour;
                case E_DamageTypes.Ember:
                    return emberColour;

                case E_DamageTypes.Healing:
                    return healColour;
                case E_DamageTypes.Shield:
                    return shieldColour;
                case E_DamageTypes.Arcana:
                    return arcanaColour;
                default:
                    return defaultColour;
            }
        }

        #endregion

        #region Impact Effect

        [Header("Impact object based on damage")]
        public Object physicalObject;
        public Object perforationObject;
        public Object septicObject;
        public Object bleakObject;
        public Object staticObject;
        public Object emberObject;

        public Object healObject;
        public Object shieldObject;
        public Object arcanaObject;

        public Object defaultObject;

        Object ImpactObjectFromDamageType(E_DamageTypes damageType)
        {
            switch (damageType)
            {
                case E_DamageTypes.Physical:
                    return physicalObject;
                case E_DamageTypes.Perforation:
                    return perforationObject;
                case E_DamageTypes.Septic:
                    return septicObject;
                case E_DamageTypes.Bleak:
                    return bleakObject;
                case E_DamageTypes.Static:
                    return staticObject;
                case E_DamageTypes.Ember:
                    return emberObject;

                case E_DamageTypes.Healing:
                    return healObject;
                case E_DamageTypes.Shield:
                    return shieldObject;
                case E_DamageTypes.Arcana:
                    return arcanaObject;
                default:
                    return defaultObject;
            }
        }

        #endregion

        #endregion
    }
}