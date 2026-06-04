using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class AbilityHandler : MonoBehaviour
{
    public Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void UseAbility(AbilityDataSO abilityData)
    {
       if (entity is Player p)
       {
            if (p.CurrentBlood < abilityData.bloodCost) return;                //Don't have enough blood to use the ability
            else p.CurrentBlood -= abilityData.bloodCost;                      //Pay the blood cost
       }

       
    }
}
