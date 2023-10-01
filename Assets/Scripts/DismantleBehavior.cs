using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismantleBehavior : MonoBehaviour
{
    [SerializeField] Texture2D cursorHammer;

    public void OnClick()
    {
        // TODO: toggle Dismantle mode, change cursor back to normal if turning off Dismantle

        Cursor.SetCursor(cursorHammer, Vector2.zero, CursorMode.ForceSoftware);
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
