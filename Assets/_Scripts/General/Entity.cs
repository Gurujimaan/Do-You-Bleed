using System.Collections.Generic;
using UnityEngine;

public enum TeamID
{
    Player,
    Enemy
}

public abstract class Entity : MonoBehaviour
{
    public abstract TeamID TeamID { get; set; }
    //public abstract List<AbilityDefinition> Abilities { get; }
    public abstract float MaxHealth { get; set; }
    public abstract float CurrentHealth { get; set; }
    public abstract float Power { get; set; }
    public abstract float MovementSpeed { get; set; }
    public abstract List<AbilityDataSO> Abilities { get; set; }
    public abstract bool moveLocked { get; set; }  
    public abstract bool casting { get; set; }
    public abstract bool isInvincible { get; set; }
    public abstract Rigidbody rb { get; }
    public abstract void TakeDamage(float amount, DamageType type);
    public abstract void TakeHealing(float amount);
    public abstract void Die();
    public void SetInvincibility(bool isInvincible)
    {
        this.isInvincible = isInvincible;
    }
}
