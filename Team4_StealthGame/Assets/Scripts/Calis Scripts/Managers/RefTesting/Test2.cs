using Cali;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    [SerializeField] public GameObject testObjRef;

    private void Start()
    {
        string test = testObjRef.GetComponent<Guard_DRS>().attachedDRef.ObjName;
        print(test + "from Test2");
    }
}
