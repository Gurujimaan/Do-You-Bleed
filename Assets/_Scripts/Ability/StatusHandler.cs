using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class StatusHandler : MonoBehaviour
{
    private Entity entity;
    public List<StatusEffectSO> statusEffectsApplied = new List<StatusEffectSO>();

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void HandleStatus(StatusEffect status)
    {
        StatusEffectSO statusEffectSO = ScriptableObject.CreateInstance<StatusEffectSO>();
        statusEffectSO.statusEffect = status;
        statusEffectsApplied.Add(statusEffectSO);
        StartCoroutine(statusEffectSO.ProcStatus(entity, OnCoroutineComplete));
    }

    public void OnCoroutineComplete(StatusEffectSO status)
    {
        statusEffectsApplied.Remove(status);
    }
}

