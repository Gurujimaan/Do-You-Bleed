using System.Collections;
using UnityEngine;

public enum BuffType { Cooldown, MoveSpeed, Damage, Defense, Regen, Shield }
public enum DebuffType { MoveSpeed, Damage, Defense, Bleed, Knockback }
public enum StatusEffect{ Buff, Debuff }

[CreateAssetMenu(fileName = "StatusEffectSO", menuName = "StatusEffect")]
public class StatusEffectSO : ScriptableObject
{
    public StatusEffect status;
    public BuffType buffType;
    public DebuffType debuffType;

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
