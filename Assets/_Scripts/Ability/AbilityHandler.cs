using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Entity))]
public class AbilityHandler : MonoBehaviour
{
    public Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void UseAbility(AbilityDataSO abilityData, Vector3? targetDirection)
    {
        Vector3 dir = targetDirection?.normalized ?? Vector3.up;                //Default to up if no target direction is provided

        if (entity is Player p)
        {
            if (p.CurrentBlood < abilityData.bloodCost) return;                 //Don't have enough blood to use the ability
            else p.CurrentBlood -= abilityData.bloodCost;                       //Pay the blood cost

            if (abilityData.mouseTargeting)
            {
                Mouse mouse = Mouse.current;
                Camera camera = Camera.main;

                Ray ray = camera.ScreenPointToRay(mouse.position.ReadValue());

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    dir = (hit.point - entity.transform.position).normalized;   //Set the direction to the point where the mouse is pointing
                }
            }
        }
                     
        if (abilityData.moveLock) LockMovement(abilityData);                    //Lock movement if the ability requires it

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
    }

    private IEnumerator LockMovement(AbilityDataSO abilityData)
    {
        entity.moveLocked = true;
        yield return new WaitForSeconds(abilityData.castTime);
        Debug.Log("cast time over");
        entity.moveLocked = false;
    }

    #region Mobility

    private IEnumerator Dash(Vector3 direction, float distance, float speed, bool invincibleDuringMove, bool damagesOnDash, float dashDamage, bool damagesOnArrival, float arrivalDamage)
    {
        Vector3 startPosition = entity.transform.position;
        Vector3 targetPosition = startPosition + direction * distance;
        if (invincibleDuringMove)
        {
            entity.SetInvincibility(true);
        }
        while (Vector3.Distance(entity.transform.position, targetPosition) > 0.1f)
        {
            entity.transform.position = Vector3.MoveTowards(entity.transform.position, targetPosition, speed * Time.deltaTime);
            Debug.Log("dashing");
            yield return null;
        }
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
