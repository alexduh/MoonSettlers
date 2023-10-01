using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildBehavior : MonoBehaviour
{
    public static bool insideMoon;

    GameObject structure;
    [SerializeField] Material wireframe;
    [SerializeField] Material solid;
    [SerializeField] Sprite constructionSprite;
    Sprite origSprite;

    [SerializeField] float buildTimer = -1;
    Dictionary<string, float> buildTime;

    public void SelectStructure(GameObject selected)
    {
        if (structure)
            Destroy(structure);

        structure = Instantiate(selected);
        structure.GetComponent<SpriteRenderer>().material = wireframe;
        insideMoon = false;
    }

    bool CheckValidLocation()
    {
        return insideMoon && !structure.GetComponent<Structure>().overlappingOther;
    }

    void StartBuilding()
    {
        SpriteRenderer sr = structure.GetComponent<SpriteRenderer>();
        origSprite = sr.sprite;
        sr.sprite = constructionSprite;
        sr.material = solid;
        structure.transform.parent = GameObject.FindWithTag("Moon").transform;
        structure.GetComponent<Structure>().placed = true;
        Structure.currentlyBuilding = true;
        buildTimer = buildTime[structure.tag];

        DisableBuilding();

        // TODO: play startBuild sound effect
    }

    void BuildFinished()
    {
        structure.GetComponent<SpriteRenderer>().sprite = origSprite;
        GameManager.buildingDict[structure.tag]++;
        Structure.currentlyBuilding = false;
        structure = null;
        EnableBuilding();
    }

    void DisableBuilding()
    {
        Button b;
        foreach (Transform child in transform)
            if (b = child.GetChild(0).GetComponent<Button>())
                b.interactable = false;
    }
    void EnableBuilding()
    {
        Button b;
        foreach (Transform child in transform)
            if (b = child.GetChild(0).GetComponent<Button>())
                b.interactable = true;
    }

    void DisableAdvancedBuildings()
    {
        Button b;
        foreach (Transform child in transform)
        {
            b = child.GetChild(0).GetComponent<Button>();
            if (b && b.tag == "Advanced")
                b.interactable = false;
        }
    }

    void EnableAdvancedBuildings()
    {
        Button b;
        foreach (Transform child in transform)
        {
            b = child.GetChild(0).GetComponent<Button>();
            if (b && b.tag == "Advanced")
                b.interactable = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buildTime = new Dictionary<string, float>()
        {
            { "Headquarters", 10 },
            { "Greenhouse", 10 },
            { "Beacon", 10 },
            { "ResearchLab", 10 },
            { "SynthesisReactor", 10 },
            { "Hospital", 10 },
            { "DefenseTurret", 10 },
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.buildingDict["ResearchLab"] == 0)
            DisableAdvancedBuildings();
        else
            EnableAdvancedBuildings();

        if (!structure)
            return;

        if (Structure.currentlyBuilding) {
            buildTimer -= Time.deltaTime * GameManager.population;
            if (buildTimer <= 0)
                BuildFinished();

            return;
        }

        if (CheckValidLocation())
            structure.GetComponent<SpriteRenderer>().material.color = Color.green;
        else
            structure.GetComponent<SpriteRenderer>().material.color = Color.red;

        if (Input.GetMouseButtonDown(0))
        {
            if (CheckValidLocation())
            {
                StartBuilding();
            }
            else
            {
                
                // TODO: play error sound effect, display error message
            }
        }
        if (Input.GetMouseButtonDown(1))
            Destroy(structure);
    }
}
