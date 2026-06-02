using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class StatusHandler : MonoBehaviour
{
    public Entity entity;
    public List<StatusEffectSO> statusEffectsApplied;

    public void HandleStatus(StatusEffectSO status)
    {
        statusEffectsApplied.Add(status);
        StartCoroutine(status.ProcStatus(entity, OnCoroutineComplete));
    }

    public void OnCoroutineComplete(StatusEffectSO status)
    {
        statusEffectsApplied.Remove(status);
    }
}

