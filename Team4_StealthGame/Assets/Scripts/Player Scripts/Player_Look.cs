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

    // look stats
    [SerializeField] float lookSensitivityX = 1.0f;
    [SerializeField] float lookSensitivityY = 1.0f; // Vertical sensitivity if needed (
    public Vector3 cursorLocation;

    // private look variables
    Vector2 lookInput;
    float xRotation;
    private int floorMask;
    private float camRayLength = 100f;
    private Rigidbody playerRigidbody;
    Camera cam;

    private AimingReticle attachedReticle;


    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
        attachedReticle = gameObject.GetComponentInChildren<AimingReticle>();
    }

    // Update is called once per frame
    void Update()
    {
        //Look();
        Turning();
        //cursorLocation = attachedReticle.GetCursorLocation();
    }

    void Look()
    {
        /*float lookX = lookInput.x * lookSensitivityX * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * lookX);*/
        //centerPoint.LookAt(cursorLocation);
    }

    void Turning()
    {
        /*Vector3 mousePos = Mouse.current.position.ReadValue(); // new input system

        Ray camRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }*/

        //transform.LookAt((attachedReticle.transform.forward * 2));
    }



    public void OnLook(InputValue v)
    {
        lookInput = v.Get<Vector2>();
    }
}
