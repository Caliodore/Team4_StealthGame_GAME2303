using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zhamanta;

/****************** Summary of Player Movement ******************/

// This script holds all player movement logic

/*****************************************************************/


public class Player_Look : MonoBehaviour
{
    // Center point Refrence
    [SerializeField] Transform centerPoint;

    // private look variables
    private int floorMask;
    private float camRayLength = 100f;
    private Rigidbody playerRigidbody;
    Camera cam;



    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Turning();
    }

    void Turning()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue(); // new input system

        Ray camRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
}
