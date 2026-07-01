using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody rb;
    [SerializeField] Enemy enemy;
    [SerializeField] AbilityHandler abilityHandler;

    [HideInInspector] public Vector2 moveDir;

    [Header("Enemy")]
    public GameObject go;


    [Header("Runtime Data")]
    public bool moveLocked;
    public List<float> abilityCooldowns = new List<float>();

    void FixedUpdate()
    {
        Move();
    }

    #region Player Movement
    private void Move()
    {
        if (moveLocked)
        {
            return; // Locks controls if movement is being halted by an ability, effect, etc.
        }
    }

    /// <summary>
    /// This coroutine locks the player's movement and takes them to a designated position at a constant speed
    /// TODO: Lock the player's combat abilities and do something to avoid clipping through terrain - Jae
    /// </summary>
    /// <param name="endPosition"> The end position the player will reach </param>
    /// <param name="speed"> The constant speed the player will travel at </param>
    public IEnumerator MoveToPosition(Vector3 endPosition, float speed)
    {
        moveLocked = true;
        Vector3 startPosition = rb.transform.position;
        float totalTime = Vector3.Distance(startPosition, endPosition) / speed;
        float startTime = Time.time;
        rb.transform.LookAt(endPosition);
        while (Vector3.Distance(rb.transform.position, endPosition) >= 0.1f)
        {
            rb.MovePosition(Vector3.Lerp(startPosition, endPosition, (Time.time - startTime) / totalTime));
            yield return null;
        }

        moveLocked = false;
    }

    #endregion

    #region Player Abilities
    public IEnumerator StartCooldown(AbilityDataSO abilityDataSO)
    {
        int index = enemy.Abilities.IndexOf(abilityDataSO);
        if (index >= 0 && index < abilityCooldowns.Count)
        {
            abilityCooldowns[index] = abilityDataSO.cooldown;
            while (abilityCooldowns[index] > 0)
            {
                GameEvents.Instance.PlayerAbilityCooldown(index, abilityCooldowns[index]);
                abilityCooldowns[index] -= Time.deltaTime;
                yield return null;
            }
            abilityCooldowns[index] = 0;
            GameEvents.Instance.PlayerAbilityCooldown(index, 0);
        }
    }

    #endregion
}
