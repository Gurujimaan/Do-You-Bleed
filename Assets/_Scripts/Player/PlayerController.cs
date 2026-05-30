using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]Rigidbody rb;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Player player;

    [Header("Player Input Settings")]
    private InputAction moveAction; //WASD

    [HideInInspector] public Vector2 moveInput;

    [Header("Player")]
    public GameObject go;

    [Header("Camera")]
    public Camera cam;

    [Header("Runtime Data")]
    private bool moveLocked;

    private void OnEnable()
    { 
        EnableInputs();
    }

    void FixedUpdate()
    {
        Move();
    }


    #region Player Movement

    private void Move()
    {
        if (moveLocked)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return; // Locks controls if movement is being halted by an ability, effect, etc.
        }

        Vector3 camForward = cam.transform.forward; //Get directions based on camera rotation and ignore y axis to prevent flying
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;
        rb.linearVelocity = moveDir * player.MovementSpeed;

        bool isMoving = moveDir.magnitude > 0.1f;

        //Turning
        if (!isMoving) return;   //don't rotate if not moving
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        Vector3 euler = targetRotation.eulerAngles;
        targetRotation = Quaternion.Euler(0, euler.y, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4 * Time.fixedDeltaTime);//rotate the player to face the direction they are moving
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


    #region Input System Callbacks

    private void EnableInputs()                                         //enables the input system and sets up callbacks for player actions
    {
        moveAction = playerInput.actions["Move"];

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
    }


    private void OnDisable()                                           //destroys input system callback on destroy to prevent memory leaks 
    {
        if (moveAction != null)
        {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
        }
    }

    //multiple functions that read input and set variables accordibly
    public void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                moveInput = context.ReadValue<Vector2>();
                break;
            case InputActionPhase.Canceled:
                moveInput = Vector2.zero;
                break;
        }
    }
    #endregion
}
