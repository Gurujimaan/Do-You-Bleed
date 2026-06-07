using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Player player;
    [SerializeField] AbilityHandler abilityHandler;

    [Header("Player Input Settings")]
    private InputAction moveAction;      // WASD
    private InputAction primaryAction;   // Left Click
    private InputAction secondaryAction; // Right Click
    private InputAction tertiaryAction;  // Q
    private InputAction utilityAction;   // Spacebar
    private InputAction interactAction;  // E
    private InputAction healAction;      // R

    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool primaryInput;
    [HideInInspector] public bool secondaryInput;
    [HideInInspector] public bool tertiaryInput;
    [HideInInspector] public bool utilityInput;
    [HideInInspector] public bool interactInput;
    [HideInInspector] public bool healInput;

    [Header("Player")]
    public GameObject go;

    [Header("Camera")]
    public Camera cam;

    [Header("Runtime Data")]
    public bool moveLocked;

    private void OnEnable()
    { 
        EnableInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Abilities();
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

    #region Player Abilities

    private void Abilities()
    {
        if (utilityInput)
        {
            abilityHandler.UseAbility(player.Abilities[3]);
        }
        else if (primaryInput)
        {
            abilityHandler.UseAbility(player.Abilities[0]);
        }
        else if (secondaryInput)
        {
            abilityHandler.UseAbility(player.Abilities[1]);
        }
        else if (tertiaryInput)
        {
            abilityHandler.UseAbility(player.Abilities[2]);
        }
    }

    #endregion

    #region Input System Callbacks

    private void EnableInputs()                                         //enables the input system and sets up callbacks for player actions
    {
        moveAction = playerInput.actions["Move"];
        primaryAction = playerInput.actions["Primary"];
        secondaryAction = playerInput.actions["Secondary"];
        tertiaryAction = playerInput.actions["Tertiary"];
        utilityAction = playerInput.actions["Utility"];
        interactAction = playerInput.actions["Interact"];
        healAction = playerInput.actions["Heal"];

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        primaryAction.performed += context => primaryInput = true;
        primaryAction.canceled += context => primaryInput = false;

        secondaryAction.performed += context => secondaryInput = true;
        secondaryAction.canceled += context => secondaryInput = false;

        tertiaryAction.performed += context => tertiaryInput = true;
        tertiaryAction.canceled += context => tertiaryInput = false;

        utilityAction.performed += context => utilityInput = true;
        utilityAction.canceled += context => utilityInput = false;

        interactAction.performed += context => interactInput = true;
        interactAction.canceled += context => interactInput = false;

        healAction.performed += context => healInput = true;
        healAction.canceled += context => healInput = false;
    }


    private void OnDisable()                                           //destroys input system callback on destroy to prevent memory leaks 
    {
        if (moveAction != null)
        {
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;
        }

        if (primaryAction != null)
        {
            primaryAction.performed -= context => primaryInput = true;
            primaryAction.canceled -= context => primaryInput = false;
        }

        if (secondaryAction != null)
        {
            secondaryAction.performed -= context => secondaryInput = true;
            secondaryAction.canceled -= context => secondaryInput = false;
        }

        if (tertiaryAction != null)
        {
            tertiaryAction.performed -= context => tertiaryInput = true;
            tertiaryAction.canceled -= context => tertiaryInput = false;
        }

        if (utilityAction != null)
        {
            utilityAction.performed -= context => utilityInput = true;
            utilityAction.canceled -= context => utilityInput = false;
        }

        if (interactAction != null)
        {
            interactAction.performed -= context => interactInput = true;
            interactAction.canceled -= context => interactInput = false;
        }

        if (healAction != null)
        {
            healAction.performed -= context => healInput = true;
            healAction.canceled -= context => healInput = false;
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
