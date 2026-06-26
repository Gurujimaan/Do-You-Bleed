using System;
using UnityEngine;
using System.Collections.Generic;


public class GameEvents : Singleton<GameEvents>
{
    public event Action<float> OnPlayerHealthChanged;
    public event Action<float> OnPlayerBloodChanged;
    public event Action<List<AbilityDataSO>> OnPlayerAbilitiesChanged;
    public event Action<int, float> OnPlayerAbilityCooldown;

    public void PlayerHealthChanged(float newHealth)
    {
        OnPlayerHealthChanged?.Invoke(newHealth);
    }

    public void PlayerBloodChanged(float newBlood)
    {
        OnPlayerBloodChanged?.Invoke(newBlood);
    }

    public void PlayerAbilitiesChanged(List<AbilityDataSO> newAbilities)
    {
        OnPlayerAbilitiesChanged?.Invoke(newAbilities);
    }

    public void PlayerAbilityCooldown(int abilityIndex, float cooldownTime)
    {
        OnPlayerAbilityCooldown?.Invoke(abilityIndex, cooldownTime);
    }
}
