using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Structure : MonoBehaviour
{
    public GameObject dismantleSound;

    Vector2 pos;
    public bool currentlyBuilding = false;
    public bool overlappingOther = false;
    private bool overlappingStructure = false;

    public bool placed = false;

    private BuildBehavior buildBehavior;

    private Color origColor = Color.clear;
    SpriteRenderer sr;

    protected Slider buildProgress;

    // Start is called before the first frame update
    protected void Start()
    {
        buildBehavior = GameObject.FindWithTag("BuildHandler").GetComponent<BuildBehavior>();
        buildProgress = FindObjectOfType<Slider>(true);
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

        Instantiate(dismantleSound);
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
            overlappingStructure = true;

        overlappingOther = overlappingStructure;
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Structure>())
            overlappingStructure = false;
    }

    void UpdateBuildProgress()
    {
        float buildTime = buildBehavior.buildTime[gameObject.tag];
        buildProgress.maxValue = buildTime;
        buildProgress.value = buildTime - buildBehavior.buildTimer;
        buildProgress.gameObject.SetActive(true);
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

        if (currentlyBuilding)
            UpdateBuildProgress();
    }
}
