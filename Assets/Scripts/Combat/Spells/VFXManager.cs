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
    public class VFXManager : MonoBehaviour
    {
        #region Setup

        public static VFXManager instance;
        public Vector2 middlePositionOffset;
        public float projectileSpeed = 0.4f;
        public float speedCalculationMultiplier = 700;

        private void Start()
        {
            instance = this;
        }

        #endregion

        #region Spell Logic

        public void AffectSelfDelay(Spell spellRef, Character caster, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, Vector2 spawnPosition, float delay, bool empowered, bool weakened)
        {
            StartCoroutine(IDelayAffectSelf(spellRef, caster, spell, effectType, cardsDiscarded, removedStatuses, spawnPosition, delay, empowered, weakened));
        }

        IEnumerator IDelayAffectSelf(Spell spellRef, Character caster, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, Vector2 spawnPosition, float delay, bool empowered, bool weakened)
        {
            yield return new WaitForSeconds(delay);
            float effectDelay = QueryTime(spawnPosition, caster.transform.position);
            VFXManager.instance.SpawnProjectile(caster.transform.position, spawnPosition, caster.transform.position, spellRef.projectileObject, spellRef.trailColor, spellRef.impactObject, effectType);
            yield return new WaitForSeconds(effectDelay);
            spellRef.AffectSelf(caster, spell, effectType, cardsDiscarded, removedStatuses, empowered, weakened);
        }

        public void AffectTargetDelay(Spell spellRef, Character caster, Character target, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, Vector2 spawnPosition, float delay, bool empowered, bool weakened)
        {
            StartCoroutine(IDelayAffectTarget(spellRef, caster, target, spell, effectType, cardsDiscarded, removedStatuses, spawnPosition, delay, empowered, weakened));
        }

        IEnumerator IDelayAffectTarget(Spell spellRef, Character caster, Character target, CombatHelperFunctions.SpellModule spell, E_DamageTypes effectType, int cardsDiscarded, int removedStatuses, Vector2 spawnPosition, float delay, bool empowered, bool weakened)
        {
            yield return new WaitForSeconds(delay);
            float effectDelay = QueryTime(spawnPosition, target.transform.position);
            VFXManager.instance.SpawnProjectile(caster.transform.position, spawnPosition, target.transform.position, spellRef.projectileObject, spellRef.trailColor, spellRef.impactObject, effectType);
            yield return new WaitForSeconds(effectDelay);
            spellRef.AffectTarget(caster, target, spell, effectType, cardsDiscarded, removedStatuses, empowered, weakened);
        }

        public float QueryTime(Vector2 spawnPosition, Vector2 targetPosition)
        {
            List<Vector2> movementPositions = new List<Vector2>();
            movementPositions.Add(spawnPosition);
            movementPositions.Add(spawnPosition + middlePositionOffset);
            movementPositions.Add(targetPosition);

            //Calculate and return delay (T=D/S)
            float distance = 0;

            for (int i = 0; i < movementPositions.Count - 1; i++)
            {
                distance += Vector2.Distance(movementPositions[i], movementPositions[i + 1]);
            }

            float time = distance / (projectileSpeed * speedCalculationMultiplier);
            //Debug.Log(time);
            time += Time.fixedDeltaTime / projectileSpeed; //Instead of 1, use the time between frames
            //float fixedFrameTime = Time.fixedDeltaTime;
            return time;
        }

        #endregion

        #region VFX

        public void SpawnProjectile(Vector2 spawnPosition, Vector2 midPosition, Vector2 targetPosition, Object projectileRef, Color trailColor, Object impactRef, E_DamageTypes damageType)
        {
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
                Debug.Log("2nd attempt");
                projectileObject = Instantiate(projectileRef, this.gameObject.transform) as GameObject;
                //return;
            }

            if (projectileObject == null)
            {
                Debug.Log("No game object spawned, returning");
                return;
            }

            projectileObject.transform.position = spawnPosition;

            ProjectileMovement projectileMovement = projectileObject.GetComponent<ProjectileMovement>();

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

            projectileMovement.Setup(color, impactFX);

            List<Vector2> movementPositions = new List<Vector2>();
            movementPositions.Add(spawnPosition);
            movementPositions.Add(midPosition);
            movementPositions.Add(targetPosition);

            projectileMovement.MoveToPositions(projectileSpeed, movementPositions);
        }

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

        #region Colour

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