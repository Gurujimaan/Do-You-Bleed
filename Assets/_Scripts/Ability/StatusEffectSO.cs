using System.Collections;
using UnityEngine;

public enum StatusEffect
{ 
    Speed_Buff, Damage_Buff, Defense_Buff, Regen_Buff, Shield_Buff, CD_Buff,
    Speed_Debuff, Damage_Debuff, Bleed_Debuff, Knockback_Debuff
}

[CreateAssetMenu(fileName = "StatusEffectSO", menuName = "StatusEffect")]
public class StatusEffectSO : ScriptableObject
{
    public StatusEffect statusEffect;

    public Sprite icon;
    public float statusAmount;
    public bool isPercent;
    public float statusDuration;

    public IEnumerator ProcStatus(Entity target, System.Action<StatusEffectSO> onComplete)
    {



        float duration = statusDuration;
        while (duration > 0)
        {
            yield return new WaitForSeconds(0.5f);
            duration -= 0.5f;
        }

        onComplete?.Invoke(this);
        yield return null;
    }
}
