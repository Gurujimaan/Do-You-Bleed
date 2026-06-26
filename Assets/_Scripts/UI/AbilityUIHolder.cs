using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIHolder : MonoBehaviour
{
    [Header("References")]
    public Image icon;
    public Image border;
    public Image cooldownOverlay;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI bloodCost;

    private AbilityDataSO _abilityData;
    [Header("Ability")]
    public AbilityDataSO abilityData
    {
        get => _abilityData;
        set
        {
            _abilityData = value;
            UpdateAbilityUI();
        }
    }

    void UpdateAbilityUI()
    {
        icon.sprite = abilityData.icon;

        if (abilityData.bloodCost <= 0)
        {
            bloodCost.gameObject.SetActive(false);
        }
        bloodCost.text = abilityData.bloodCost.ToString();
    }

    public void SetCooldown(float cooldownTime)
    {
        cooldownText.text = Mathf.CeilToInt(cooldownTime).ToString();
    }
}
