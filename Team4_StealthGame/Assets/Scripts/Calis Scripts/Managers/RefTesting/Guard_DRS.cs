using UnityEngine;
using Cali;

public class Guard_DRS : MonoBehaviour
{
    public DictRef attachedDRef;
    public MonoBehaviour attachedTypeScript;

    private void Awake()
    {
        attachedDRef = new DictRef(attachedTypeScript, 0);
        print(attachedDRef);
    }
}
