using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Structure : MonoBehaviour
{
    Vector2 pos;
    public bool currentlyBuilding = false;
    public bool overlappingOther = false;
    public bool placed = false;

    private BuildBehavior buildBehavior;

    private Color origColor = Color.clear;
    SpriteRenderer sr;

    // Start is called before the first frame update
    protected void Start()
    {
        buildBehavior = GameObject.FindWithTag("BuildHandler").GetComponent<BuildBehavior>();
    }

    public void Dismantle()
    {
        if (!DismantleBehavior.dismantleMode)
            return;
        if (!currentlyBuilding)
            GameManager.buildingDict[gameObject.tag]--;
        else
        {
            buildBehavior.EnableBuilding();
            currentlyBuilding = false;
        }

        // TODO: play dismantle sound effect!
        Destroy(gameObject);
    }

    public void Highlight()
    {
        if (!DismantleBehavior.dismantleMode)
            return;

        sr = GetComponent<SpriteRenderer>();
        origColor = sr.material.color;
        sr.material.color = Color.red;
    }

    public void UnHighlight()
    {
        if (origColor == Color.clear)
            return;

        sr = GetComponent<SpriteRenderer>();
        sr.material.color = origColor;
        origColor = Color.clear;
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
