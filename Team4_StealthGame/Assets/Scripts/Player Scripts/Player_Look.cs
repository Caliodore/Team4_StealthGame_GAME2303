using UnityEngine;
using UnityEngine.InputSystem;


/****************** Summary of Player Movement ******************/

// This script holds all player movement logic

/*****************************************************************/


public class Player_Look : MonoBehaviour
{
    // look stats
    [SerializeField] float lookSensitivityX = 1.0f;
    [SerializeField] float lookSensitivityY = 1.0f; // Vertical sensitivity if needed (

    // private look variables
    Vector2 lookInput;
    float xRotation;

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    void Look()
    {

        float lookX = lookInput.x * lookSensitivityX * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * lookX);
    }

    public void OnLook(InputValue v)
    {
        lookInput = v.Get<Vector2>();
    }
}
