using System;
using UnityEngine;


public class GameEvents : Singleton<GameEvents>
{
    public event Action<float> OnPlayerHealthChanged;
    public event Action<float> OnPlayerBloodChanged;

    public void PlayerHealthChanged(float newHealth)
    {
        OnPlayerHealthChanged?.Invoke(newHealth);
    }

    public void PlayerBloodChanged(float newBlood)
    {
        OnPlayerBloodChanged?.Invoke(newBlood);
    }
}
