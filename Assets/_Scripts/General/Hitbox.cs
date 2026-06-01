using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Hitbox : MonoBehaviour
{
    [Header("Hitbox Settings")]
    [SerializeField]private Entity owner;
    public TeamID TeamID => owner.TeamID;

    public int DamageAmount = 10;

    void Start()
    {
        
    }
}
