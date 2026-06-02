using NaughtyAttributes;
using UnityEngine;


public enum DamageType { Blood, Fire, Ice, Lightning, Poison, Light }
public enum AOEShape { None, Circle, Rectangle, Cone, Line }

public enum MobilityType { Dash, Teleport, Leap }

[CreateAssetMenu(menuName = "Abilities/Base Ability")]
public class AbilityDataSO : ScriptableObject
{
    [Header("Info")]
    public string abilityName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Cost & Cooldown")]
    public float manaCost;
    public float manaCostHeld; 
    public float cooldown;

    [Header("General")]
    public float range;
    public bool moveLock;
    public bool mouseTargeting;
    public ProjectileSO projectile;

    [Header("Damage")]
    public float damage;
    public DamageType damageType;

    [Header("Charge")]
    public float chargeTime;                                                       // 0 = instant
    [ShowIf("chargeTime")] public bool canMoveWhileCharging;
    [ShowIf("chargeTime")] public float movementSpeedWhileCharging;                // 1 = normal speed, 0.5 = half speed, etc.

    [Header("AOE")]
    public bool AOE;
    [ShowIf("AOE")] public AOEShape aoeShape;
    [ShowIf("AOE")] public float aoeRadius;          // for Circle
    [ShowIf("AOE")] public Vector2 aoeSize;          // for Rectangle / Cone
    [ShowIf("AOE")] public float aoeAngle;           // for Cone

    [Header("Buff")]
    public bool appliesBuff;
    [ShowIf("appliesBuff")] public StatusEffectSO buffEffect;

    [Header("Debuff")]
    public bool appliesDebuff;
    [ShowIf("appliesDebuff")] public StatusEffectSO debuffEffect;

    [Header("Mobility")]
    public bool Mobility;
    [ShowIf("Mobility")] public MobilityType mobilityType;
    [ShowIf("Mobility")] public float distance;
    [ShowIf("Mobility")] public float speed;
    [ShowIf("Mobility")] public bool invincibleDuringMove; // i-frames on dash
    [ShowIf("Mobility")] public bool canPassThroughEnemies;   // teleport through enemies
    [ShowIf("Mobility")] public bool damagesOnDash;         // dash into enemies deals damage
    [ShowIf("Mobility")] public float dashDamage;
    [ShowIf("Mobility")] public bool damagesOnArrival;     // blink into enemies deals damage
    [ShowIf("Mobility")] public float arrivalDamage;

    // Override
    public virtual void Execute(GameObject caster, Vector2 targetPosition) { }
}