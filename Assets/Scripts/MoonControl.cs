using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoonControl : MonoBehaviour
{
    Collider2D col;

    [SerializeField] Texture2D cursorGrabbyHand;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Settler")
            return;

        if (col.bounds.Contains(other.bounds.max) && col.bounds.Contains(other.bounds.min))
            BuildBehavior.insideMoon = true;
        else
            BuildBehavior.insideMoon = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: change this to only work when right-clicking the moon
        if (Input.GetMouseButtonDown(1))
            Cursor.SetCursor(cursorGrabbyHand, Vector2.zero, CursorMode.ForceSoftware);
        if (Input.GetMouseButtonUp(1))
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);

        if (Input.GetMouseButton(1))
            transform.rotation *= Quaternion.Euler(Input.GetAxis("Mouse X") * new Vector3(0, 0, -1));
    }
}
