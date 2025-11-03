using UnityEngine;
using UnityEngine.InputSystem;

namespace Zhamanta
{
    public class AimingReticle : MonoBehaviour
    {
        [SerializeField] Camera mainCam;
        private Vector3 mousePos;

        private void Update()
        {
            mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            Vector3 rotation = mousePos - transform.position;

            float roty = Mathf.Atan2(rotation.z, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, -roty, 0);
        }
    }
}

