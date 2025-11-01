using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

/****************** Summary of Player Movement ******************/

// This script holds all player movement logic

/*****************************************************************/

public class Player_Movement : MonoBehaviour
{
    // references
    CharacterController controller;

    // movement variables for unity events
    public bool isMakingSound;
    public bool isMoving;

    // movement stats
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float gravity = -9.81f;

    // private movement variables
    Vector3 velocity;
    bool grounded;
    Vector2 movementInput;
    bool sprintInput = false;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        // always go back to "no velocity"
        // "velocity" is for movement speed that we gain in addition to our movement (falling, knockback, etc.)
        Vector3 noVelocity = new Vector3(0, velocity.y, 0);
        velocity = Vector3.Lerp(velocity, noVelocity, 5 * Time.deltaTime);


        // player sneak logic
        /*if (!sprintInput || movementInput.magnitude == 0)
        {
            isMakingSound = false;
        }
        else if (sprintInput && movementInput.magnitude > 0)
        {
            isMakingSound = true;
        }*/

    }
    void Movement()
    {


        grounded = controller.isGrounded;

        if (grounded && velocity.y < 0)
        {
            velocity.y = -1;// -0.5f;
        }

        Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;
        controller.Move(move * movementSpeed * (sprintInput ? 2 : 1) * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnMovement(InputValue v)
    {

        print("OnMovement Called!");
        movementInput = v.Get<Vector2>();

        /*if (movementInput.magnitude > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }*/
    }


    bool OnSprint(InputValue v)
    {
        return sprintInput = v.isPressed;
    }








    // Character Controller can't use OnCollisionEnter :D thanks Unity
   /* private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Damager>())
        {
            var collisionPoint = hit.collider.ClosestPoint(transform.position);
            var knockbackAngle = (transform.position - collisionPoint).normalized;
            velocity = (20 * knockbackAngle);
        }

        if (hit.gameObject.GetComponent<KillZone>())
        {
            Respawn();
        }
    } */
    }
