using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;

public enum DamageType { Blood, Fire, Ice, Lightning, Poison, Light }
public enum AOEShape { None, Circle, Rectangle, Cone, Line }
public enum MobilityType { Dash, Teleport }

[CreateAssetMenu(menuName = "Abilities/Base Ability")]
public class AbilityDataSO : ScriptableObject
{
    [Header("Info")]
    public string abilityName;
    public Sprite icon;
    [TextArea] public string description;

    [Header("Cost & Cooldown")]
    public float bloodCost;
    public float bloodCostHeld; 
    public float cooldown;

    [Header("General")]
    public float range;                 //enemies only
    public bool moveLock;
    public float castTime;
    public bool mouseTargeting;
    [HideIf("mouseTargeting")] public bool travelWithEntity;
    public ProjectileSO projectile;

    [Header("Damage")]
    public float damage;
    public DamageType damageType;

    private bool IsCharging => chargeTime != 0;
    [Header("Charge")]
    public float chargeTime;                                                  // 0 = instant
    [ShowIf("IsCharging")] public bool canMoveWhileCharging;
    [ShowIf("IsCharging")] public float movementSpeedWhileCharging;           // 1 = normal speed, 0.5 = half speed, etc.

    private bool HasAOE => aoeShape != AOEShape.None;
    [Header("AOE")]
    public AOEShape aoeShape;
    [ShowIf("HasAOE")] public float aoeRadius;                  // for Circle
    [ShowIf("HasAOE")] public Vector2 aoeSize;                  // for Rectangle / Cone
    [ShowIf("HasAOE")] public float aoeAngle;                   // for Cone

    [Header("Buff")]
    public bool appliesBuff;
    [ShowIf("appliesBuff")] public List<StatusEffectSO> buffEffect;

    [Header("Debuff")]
    public bool appliesDebuff;
    [ShowIf("appliesDebuff")] public List<StatusEffectSO> debuffEffect;

    [Header("Mobility")]
    public bool Mobility;
    [ShowIf("Mobility")] public MobilityType mobilityType;
    [ShowIf("Mobility")] public float distance;
    [ShowIf("Mobility")] public float speed;
    [ShowIf("Mobility")] public bool invincibleDuringMove;                     // i-frames on dash
    [ShowIf("Mobility")] public bool damagesOnDash;                            // dash into enemies deals damage
    [ShowIf("Mobility")] public float dashDamage;
    [ShowIf("Mobility")] public bool damagesOnArrival;                         // blink into enemies deals damage
    [ShowIf("Mobility")] public float arrivalDamage;
}