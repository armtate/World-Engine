using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D crosshairTexture;

    private void Start()
    {
        Vector2 hotspot = new Vector2(
            crosshairTexture.width / 2,
            crosshairTexture.height / 2
        );

        Cursor.SetCursor(crosshairTexture, hotspot, CursorMode.Auto);
        Cursor.visible = true;
    }
}