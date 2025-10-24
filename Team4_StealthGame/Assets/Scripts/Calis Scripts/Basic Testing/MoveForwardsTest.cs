using UnityEngine;

public class MoveForwardsTest : MonoBehaviour
{
    public float moveSpeed;
    public float outputForce;
    Rigidbody attachedRB;
    Vector3 relFor;

    private void Awake()
    {
        attachedRB = GetComponent<Rigidbody>();
        relFor = transform.forward;
        relFor *= moveSpeed;
    }

    private void FixedUpdate()
    {
        if (attachedRB != null) 
        {
            outputForce = relFor.magnitude;
            attachedRB.AddForce(relFor,ForceMode.Force);
        }
    }
}
