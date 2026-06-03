using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("References")]
    public Slider health;
    public Slider blood;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        GameEvents.Instance.OnPlayerHealthChanged += UpdateHealth;
        GameEvents.Instance.OnPlayerBloodChanged += UpdateBlood;
    }

    // Update is called once per frame
    private void UpdateHealth(float newHealth)
    {
        health.value = newHealth;
    }

    private void UpdateBlood(float newBlood)
    {
        blood.value = newBlood;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnPlayerHealthChanged -= UpdateHealth;
        GameEvents.Instance.OnPlayerBloodChanged -= UpdateBlood;
    }
}
