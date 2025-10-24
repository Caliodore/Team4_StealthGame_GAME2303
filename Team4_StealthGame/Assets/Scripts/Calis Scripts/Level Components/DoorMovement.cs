using Cali;
using System.Collections;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    [Header("Component Refs")]
    [SerializeField] HingeJoint pivotHinge;
    [SerializeField] Rigidbody swingingRigidbody;
    [SerializeField] GameObject hingeAnchorObj;

    [Header("Internal Vars")]
    public Vector3 startPosition;
    public Quaternion startRotation;
    
    private Vector3 hingeCenterVec;

    public bool canSwing;
    public DoorLogic attachedLogic;

    public RigidbodyConstraints lockedConstraints;
    public RigidbodyConstraints normalConstraints;

    [Header("Testing Vars")]
    public bool testingMode;
    public bool forwardTest;
    public bool relTest;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void Start()
    {
        //attachedLogic = gameObject.GetComponentInParent<DoorLogic>();
        pivotHinge = GetComponentInChildren<HingeJoint>();
        hingeAnchorObj = GetComponentInChildren<HingeFlag>().gameObject;
        swingingRigidbody = pivotHinge.gameObject.GetComponent<Rigidbody>();
        canSwing = true;
        normalConstraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        lockedConstraints = RigidbodyConstraints.FreezeAll;

        hingeCenterVec = hingeAnchorObj.transform.forward;
    }
    private void Update()
    {
        if(testingMode)
        {
            if(forwardTest)
                Debug.DrawRay(hingeAnchorObj.transform.position, hingeAnchorObj.transform.forward * 5, Color.red, 1f);
        }
    }

    public void ToggleDoorSwinging(bool freeOrLocked)
    { 
        if(freeOrLocked)
            swingingRigidbody.constraints = normalConstraints;
        else
            swingingRigidbody.constraints = lockedConstraints;
    }

    /// <summary>
    /// Method mainly to be called by DoorLogic.PlayerInteractHandler to close an open door without locking it.
    /// </summary>
    public void CloseOpenDoor()
    { 
        
    }

    /// <summary>
    /// Method to be called to determine which way the door needs to rotate to go back to being closed.
    /// </summary>
    private void DetermineRotationDirection()
    {
        Vector3 vecHingeToDoor = swingingRigidbody.transform.position - hingeAnchorObj.transform.position;
        vecHingeToDoor = new Vector3(vecHingeToDoor.x, 0, vecHingeToDoor.z).normalized;
        float dotCheck = Vector3.Dot(hingeCenterVec, vecHingeToDoor);
        //Dot product = 1 means they are the same direction. We want some slight error but to correct it to 1 eventually.
        //Vector3.Angle(hingeCenterVec, vecHingeToDoor);
    }

    /// <summary>
    /// Coroutine to handle interpolating the door's position and rotation back to its default.
    /// </summary>
    IEnumerator LerpToClose()
    {
        
        yield return null;    
    }

}
