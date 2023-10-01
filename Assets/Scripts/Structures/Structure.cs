using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    Vector2 pos;
    public static bool currentlyBuilding = false;
    public bool overlappingOther = false;
    public bool placed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Highlight()
    {
        if (!DismantleBehavior.dismantleMode)
            return;

        // TODO: highlight building to indicate dismantling is possible
    }

    protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Structure>())
            overlappingOther = true;
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Structure>())
            overlappingOther = false;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!currentlyBuilding && !placed)
        {
            pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
            transform.position = pos;
        }
    }
}
