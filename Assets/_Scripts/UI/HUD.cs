using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("References")]
    public Slider health;
    public Slider blood;
    public List<AbilityUIHolder> abilityUIHolders;

    void OnEnable()
    {
        GameEvents.Instance.OnPlayerHealthChanged += UpdateHealth;
        GameEvents.Instance.OnPlayerBloodChanged += UpdateBlood;
        GameEvents.Instance.OnPlayerAbilitiesChanged += UpdatePlayerAbilities;
    }

    private void UpdateHealth(float newHealth)
    {
        health.value = newHealth;
    }

    private void UpdateBlood(float newBlood)
    {
        blood.value = newBlood;
    }

    private void UpdatePlayerAbilities(List<AbilityDataSO> newAbilities)
    {
        for (int i = 0; i < newAbilities.Count; i++)
        {
            abilityUIHolders[i].abilityData = newAbilities[i];
        }
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnPlayerHealthChanged -= UpdateHealth;
        GameEvents.Instance.OnPlayerBloodChanged -= UpdateBlood;
        GameEvents.Instance.OnPlayerAbilitiesChanged -= UpdatePlayerAbilities;
    }
}
