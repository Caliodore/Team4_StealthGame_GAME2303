using UnityEngine;
using UnityEngine.InputSystem;

public class AimingReticle : MonoBehaviour
{
    [SerializeField] Camera mainCam;
    private Vector3 mousePos;
    public Quaternion outputAimRotation;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector3 rotation = mousePos - transform.position;

        float roty = Mathf.Atan2(rotation.z, rotation.x) * Mathf.Rad2Deg;

        outputAimRotation  = Quaternion.Euler(0, -roty, 0);
        transform.rotation = outputAimRotation;
    }

    public Vector3 GetCursorLocation()
    { 
        return mousePos;    
    }
}
