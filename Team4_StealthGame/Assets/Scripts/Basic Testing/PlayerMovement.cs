using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerWalkSpeed, playerRunSpeed, playerSneakSpeed, currentPlayerSpeed;

    [SerializeField] Rigidbody attachedRB;

    Vector2 inputMovementVector;
    Vector3 outputMovementVector;
    Vector3 outputMovementVectorScaled;

    public bool isSneaking, isMoving, isRunning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        currentPlayerSpeed = playerWalkSpeed;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        isMoving = true;
        inputMovementVector = ctx.ReadValue<Vector2>();
        if(ctx.canceled)
            isMoving = false;
    }

    private void FixedUpdate()
    {
        if(isMoving)
        { 
            outputMovementVector = new Vector3(inputMovementVector.x, 0, inputMovementVector.y);
            outputMovementVectorScaled = outputMovementVector * Time.deltaTime * currentPlayerSpeed;
            transform.Translate(outputMovementVectorScaled); 
        }
    }
}
