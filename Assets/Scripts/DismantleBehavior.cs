using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismantleBehavior : MonoBehaviour
{
    [SerializeField] Texture2D cursorHammer;

    public static bool dismantleMode = false;

    public void ToggleMode()
    {
        if (!dismantleMode)
        {
            Cursor.SetCursor(cursorHammer, new Vector2(10, 10), CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        }

        dismantleMode = !dismantleMode;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
