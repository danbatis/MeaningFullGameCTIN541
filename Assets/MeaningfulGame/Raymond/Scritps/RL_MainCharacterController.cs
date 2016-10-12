using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RL_MainCharacterController : MonoBehaviour
{
    enum CharacterState
    {
        Idle = 0,
        Walking = 1,
    }

    private CharacterState _characterState;

    // The speed when walking
    public float walkSpeed = 2.0f;

    // The gravity for the character
    float gravity = 20.0f;
    // The gravity in controlled descent mode
    float speedSmoothing = 10.0f;
    float rotateSpeed = 500.0f;

    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
    private float lockCameraTimer = 0.0f;

    // The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    // The current vertical speed
    private float verticalSpeed = 0.0f;
    // The current x-z move speed
    private float moveSpeed = 0.0f;

    // The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags;

    // Are we moving backwards (This locks the camera to not do a 180 degree spin)
    private bool movingBack = false;
    // Is the user pressing any keys?
    private bool isMoving = false;
    // When did the user start walking (Used for going into trot after a while)
    private float walkTimeStart = 0.0f;

    private bool isControllable = true;

    private Animator anim;

    void Awake()
    {
        moveDirection = transform.TransformDirection(Vector3.forward);

        anim = GetComponent<Animator>();
    }

    void UpdateSmoothedMovementDirection()
    {
        Transform cameraTransform = Camera.main.transform;

        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float v = 0, h = 0;
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");

        // Are we moving backwards or looking backwards
        if (v < -0.2f)
            movingBack = true;
        else
            movingBack = false;

        bool wasMoving = isMoving;
        isMoving = Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;

        // Target direction relative to the camera
        Vector3 targetDirection = h * right + v * forward;

            // Lock camera for short period when transitioning moving & standing still
            lockCameraTimer += Time.deltaTime;
            if (isMoving != wasMoving)
                lockCameraTimer = 0.0f;

            // We store speed and direction seperately,
            // so that when the character stands still we still have a valid forward direction
            // moveDirection is always normalized, and we only update it if there is user input.
            if (targetDirection != Vector3.zero)
            {
                moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);

                moveDirection = moveDirection.normalized;

            }

            // Smooth the speed based on the current target direction
            float curSmooth = speedSmoothing * Time.deltaTime;

            // Choose target speed
            //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            _characterState = CharacterState.Idle;

            targetSpeed *= walkSpeed;
            _characterState = CharacterState.Walking;

            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

            // Reset walk time start when we slow down
            if (moveSpeed < walkSpeed * 0.3f)
                walkTimeStart = Time.time;
        
    }

    void ApplyGravity()
    {
        if (isControllable) // don't move player at all if not controllable.
        {
            // Apply gravity
            if (IsGrounded())
                verticalSpeed = 0.0f;
            else
                verticalSpeed -= gravity * Time.deltaTime;
        }
    }

    void Update()
    {
        // Apply gravity
        // - controlledDescent mode modifies gravity
        ApplyGravity();
        anim.SetFloat("Speed", moveSpeed);

        if (!isControllable)
            return;

        UpdateSmoothedMovementDirection();

        // Calculate actual motion
        Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0);
        movement *= Time.deltaTime;

        // Move the controller
        CharacterController controller = GetComponent<CharacterController>();

        collisionFlags = controller.Move(movement);
        transform.rotation = Quaternion.LookRotation(moveDirection);

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //	Debug.DrawRay(hit.point, hit.normal);
        if (hit.moveDirection.y > 0.01f)
            return;
    }

    float GetSpeed()
    {
        return moveSpeed;
    }

    bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    Vector3 GetDirection()
    {
        return moveDirection;
    }

    public bool IsMovingBackwards()
    {
        return movingBack;
    }

    public float GetLockCameraTimer()
    {
        return lockCameraTimer;
    }

    bool IsMoving()
    {
        return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
    }

    void Reset()
    {
        gameObject.tag = "Player";
    }

    public void SetControllable(bool val)
    {
        if (val)
        {
            isControllable = true;

        }
        else
        {
            isControllable = false;

            moveSpeed = 0.0f;
            isMoving = false;
        }
    }
}