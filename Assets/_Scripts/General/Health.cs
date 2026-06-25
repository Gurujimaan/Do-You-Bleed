using UnityEngine;

public class Health : MonoBehaviour
{
    private Entity entity;
    public TeamID TeamID => entity.TeamID;

    void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitbox hit = other.GetComponent<Hitbox>();
        TeamID otherTeam = hit.TeamID;

        if (otherTeam != entity.TeamID && !entity.isInvincible)
        {
            entity.TakeDamage(hit.DamageAmount, hit.damageType);
        }
    }
}
