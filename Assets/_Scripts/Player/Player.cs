using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class Player : Entity
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
    // ---- Base Values ----    Inspector values to initialize SyncVars (For Designers)
    public float baseMaxHealth = 100;
    public float baseCurrentHealth = 100;
    public float basePower = 10;
    public float baseMovementSpeed = 5;
    public float baseMaxBlood = 100;
    public float baseCurrentBlood = 100;

    // ---- Saved Vars ----      These are the actual values used in the game
    private float currentMaxHealth;
    private float currentHealth;
    private float currentPower;
    private float currentMovementSpeed;
    private float currentMaxBlood;
    private float currentBlood;

    // ---- Public Accessors ----   !! Only use these to access/edit the SyncVar values !!
    public override float MaxHealth
    {
        get => currentMaxHealth;
        set => currentMaxHealth = value;
    }
    public override float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            UpdatePlayerHealth();
        }
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
    public float MaxBlood
    {
        get => currentMaxBlood;
        set => currentMaxBlood = value;
    }
    public float CurrentBlood
    {
        get => currentBlood;
        set
        {
            currentBlood = value;
            UpdatePlayerBlood();
        }
    }
    #endregion

    //[Header("Attack Settings")]
    //[SerializeField] private List<AbilityDefinition> abilities;
    //public override List<AbilityDefinition> Abilities => abilities;

    [Header("References")]
    public PlayerController playerController;
    public Animator anim;

    public void Awake()
    {
        MaxHealth = baseMaxHealth;
        Power = basePower;
        MovementSpeed = baseMovementSpeed;
        MaxBlood = baseMaxBlood;
    }

    public void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentBlood = MaxBlood;
    }

    private void Update()
    {

    }

    #region API

    public override void TakeDamage(float attackPower)
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
        Debug.Log("Player died");
    }

    #endregion

    #region UI

    private void UpdatePlayerHealth()
    {
        GameEvents.Instance.PlayerHealthChanged(CurrentHealth / MaxHealth);
    }

    private void UpdatePlayerBlood()
    {
        GameEvents.Instance.PlayerBloodChanged(CurrentBlood / MaxBlood);       
    }

    public void UpdateAbilities()
    {
       
    }

    #endregion
}
