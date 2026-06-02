using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Projectile")]
public class ProjectileSO : ScriptableObject
{
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float speed;
    public float lifetime;
    public Vector3 spawn;
    public Vector3 target;

    [Header("Damage")]
    public float damage;
    public DamageType damageType;
    public List<StatusEffectSO> onHitEffects;

    [Header("Spread")]
    public int projectileAmount;
    public float spreadAngle;              // in degrees, total angle of the spread
    public bool randomizeSpread; 
}
