using UnityEngine;

namespace Zhamanta
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorTextureDefault;

        [SerializeField] private Vector3 clickPosition = Vector2.zero;

        private void Start()
        {
            Cursor.SetCursor(cursorTextureDefault, clickPosition, CursorMode.Auto);
        }
    }
}

