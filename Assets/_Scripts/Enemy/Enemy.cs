using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("State Data")]
    [SerializeField] private TeamID teamID;
    public override TeamID TeamID
    {
        get => teamID;
        set => teamID = value;
    }

    [Header("Stats")]
    #region Stats
    // ---- Base Values ----    Inspector values to initialize (For Designers)
    public float baseMaxHealth = 100;
    public float baseCurrentHealth = 100;
    public float basePower = 10;
    public float baseMovementSpeed = 5;
    public float baseBlood = 100;
    public List<AbilityDataSO> baseAbilities = new List<AbilityDataSO>();

    // ---- Saved Vars ----      These are the actual values used in the game
    private float currentMaxHealth;
    private float currentHealth;
    private float currentPower;
    private float currentMovementSpeed;
    private float currentBlood;
    private List<AbilityDataSO> currentAbilities = new List<AbilityDataSO>();

    // ---- Public Accessors ----   !! Only use these to access/edit values !!
    public override float MaxHealth
    {
        get => currentMaxHealth;
        set => currentMaxHealth = value;
    }
    public override float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }
    public override float Power
    {
        get => currentPower;
        set => currentPower = value;
    }
    public override float MovementSpeed
    {
        get => currentMovementSpeed;
        set => currentMovementSpeed = value;
    }
    public float Blood
    {
        get => currentBlood;
        set => currentBlood = value;
    }
    public override List<AbilityDataSO> Abilities
    {
        get => currentAbilities;
        set => currentAbilities = value;
    }
    public override bool moveLocked { get; set; }
    public override bool casting { get; set; }
    public override bool isInvincible { get; set; }
    public override Rigidbody rb { get; }
    #endregion

    [Header("References")]
    public AbilityHandler abilityHandler;
    public StatusHandler statusHandler;
    public Animator anim;

    public void Awake()
    {
        MaxHealth = baseMaxHealth;
        Power = basePower;
        MovementSpeed = baseMovementSpeed;
        Blood = baseBlood;
    }

    public void Start()
    {
        CurrentHealth = MaxHealth;
        Abilities = baseAbilities;
    }

    #region API

    public override void TakeDamage(float attackPower, DamageType type)
    {
        float damage = Mathf.Max(attackPower, 0);
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public override void TakeHealing(float healPower)
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + healPower);
    }

    public override void Die()
    {
        Debug.Log("Enemy died");
    }

    #endregion
}
