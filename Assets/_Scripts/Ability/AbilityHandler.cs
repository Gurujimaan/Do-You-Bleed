using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Entity))]
public class AbilityHandler : MonoBehaviour
{
    public Entity entity;
    public float targetYLevel;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void UseAbility(AbilityDataSO abilityData, Vector3 targetDirection)
    {
        Vector3 dir = new Vector3(targetDirection.x, 0, targetDirection.y).normalized;

        if (entity is Player p)
        {
            if (p.CurrentBlood < abilityData.bloodCost) return;                 //Don't have enough blood to use the ability
            else p.CurrentBlood -= abilityData.bloodCost;                       //Pay the blood cost

            if (abilityData.mouseTargeting)
            {
                Plane mathematicalPlane = new Plane(Vector3.up, new Vector3(0, targetYLevel, 0));

                Mouse mouse = Mouse.current;
                Camera camera = Camera.main;

                Ray ray = camera.ScreenPointToRay(mouse.position.ReadValue());

                if (mathematicalPlane.Raycast(ray, out float enterDistance))
                {
                    dir = (ray.GetPoint(enterDistance) - entity.transform.position).normalized;   //Set the direction to the point where the mouse is pointing
                }
            }
        }
                     
        if (abilityData.moveLock) StartCoroutine(LockMovement(abilityData));                    //Lock movement if the ability requires it

        if (abilityData.projectile != null)
        {
            GameObject.Instantiate(abilityData.projectile.projectilePrefab, entity.transform.position, Quaternion.LookRotation(dir));   //Spawn the projectile in the direction of the target
        }
        else
        {
            if (abilityData.Mobility)
            {
                if (abilityData.mobilityType == MobilityType.Dash)
                {
                    StartCoroutine(Dash(dir, abilityData.distance, abilityData.speed, abilityData.invincibleDuringMove, abilityData.damagesOnDash, abilityData.dashDamage, abilityData.damagesOnArrival, abilityData.arrivalDamage));
                }
                else if (abilityData.mobilityType == MobilityType.Teleport)
                {
                    Teleport(dir, abilityData.distance);
                }
            }
        }

        if (entity is Player pl)
        {
            StartCoroutine(pl.playerController.StartCooldown(abilityData));
        }
    }


    private IEnumerator LockMovement(AbilityDataSO abilityData)
    {
        if (entity is Player p) p.casting = true;
        entity.moveLocked = true;
        float time = abilityData.castTime;
        if (abilityData.Mobility) time = abilityData.castTime + (abilityData.distance / abilityData.speed);   //If the ability has mobility, add the time it takes to travel the distance to the cast time

        yield return new WaitForSeconds(time);
        Debug.Log("cast time over");
        entity.moveLocked = false;
        if (entity is Player pl) pl.casting = false;
    }

    #region Mobility

    private IEnumerator Dash(Vector3 direction, float distance, float speed, bool invincibleDuringMove, bool damagesOnDash, float dashDamage, bool damagesOnArrival, float arrivalDamage)
    {
        Vector3 startPosition = entity.transform.position;
        Vector3 targetPosition = startPosition + direction * distance;

        float duration = distance / speed;
        float elapsedTime = 0f;

        if (invincibleDuringMove)
        {
            entity.SetInvincibility(true);
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percent = elapsedTime / duration;
            entity.transform.position = Vector3.Lerp(startPosition, targetPosition, percent);

            yield return null;
        }
        entity.transform.position = targetPosition;

        if (invincibleDuringMove)
        {
            entity.SetInvincibility(false);
        }
    }

    private void Teleport(Vector3 direction, float distance)
    {
        Vector3 targetPosition = entity.transform.position + direction * distance;
        entity.transform.position = targetPosition;
    }

    #endregion
}
