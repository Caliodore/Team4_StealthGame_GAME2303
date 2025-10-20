using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

namespace Testing
{
    public class PlayerMovement : MonoBehaviour
    {
        // movement values
        [Header("Movement Values")]
        [SerializeField] float moveSpeed = 1.0f;

        // references
        Rigidbody rb;

        // variables
        Vector3 movementVector;

        void Start()
        {
            rb = GetComponent<Rigidbody>();

        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();

            movementVector = new Vector3(inputVector.x, 0, inputVector.y);
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = movementVector * moveSpeed;
        }
    }
}

