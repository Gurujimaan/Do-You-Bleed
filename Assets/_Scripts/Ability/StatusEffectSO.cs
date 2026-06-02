using UnityEngine;

public enum BuffType { Cooldown, MoveSpeed, Damage, Defense, Regen, Shield }
public enum DebuffType { Cooldown, MoveSpeed, Damage, Defense, Bleed, Knockback }

[CreateAssetMenu(fileName = "StatusEffectSO", menuName = "StatusEffect")]
public class StatusEffectSO : ScriptableObject
{
    public BuffType buffType;
    public DebuffType debuffType;

    public float statusAmount;
    public bool isPercent;
    public float statusDuration;
}
