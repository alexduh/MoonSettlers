using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildBehavior : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioPlayer;

    public static bool insideMoon;
    public static bool buildingSelected = false;

    GameObject structureObj;
    Structure structure;

    [SerializeField] Material wireframe;
    [SerializeField] Material solid;
    [SerializeField] Sprite constructionSprite;
    Sprite origSprite;

    [SerializeField] float buildTimer = -1;
    Dictionary<string, float> buildTime;

    public void SelectStructure(GameObject selected)
    {
        if (DismantleBehavior.dismantleMode)
            return;

        if (structureObj)
            Destroy(structureObj);

        buildingSelected = true;
        structureObj = Instantiate(selected);
        structureObj.GetComponent<SpriteRenderer>().material = wireframe;
        structure = structureObj.GetComponent<Structure>();
        insideMoon = false;
    }

    bool CheckValidLocation()
    {
        return insideMoon && !structure.overlappingOther;
    }

    void StartBuilding()
    {
        audioPlayer.clip = sounds[0];
        audioPlayer.Play();

        buildingSelected = false;
        SpriteRenderer sr = structureObj.GetComponent<SpriteRenderer>();
        origSprite = sr.sprite;
        sr.sprite = constructionSprite;
        sr.material = solid;
        structureObj.transform.parent = GameObject.FindWithTag("Moon").transform;
        structure.placed = true;
        structure.currentlyBuilding = true;
        buildTimer = buildTime[structureObj.tag];

        DisableBuilding();
    }

    void BuildFinished()
    {
        audioPlayer.clip = sounds[1];
        audioPlayer.Play();

        structureObj.GetComponent<SpriteRenderer>().sprite = origSprite;
        GameManager.buildingDict[structureObj.tag]++;
        structure.currentlyBuilding = false;
        structureObj = null;
        EnableBuilding();
    }

    void DisableBuilding()
    {
        Button b;
        foreach (Transform child in transform)
            if (b = child.GetChild(0).GetComponent<Button>())
                b.interactable = false;
    }
    public void EnableBuilding()
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
        audioPlayer = GetComponent<AudioSource>();

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
        if (GameManager.gameOver)
            return;

        if (GameManager.buildingDict["ResearchLab"] == 0)
            DisableAdvancedBuildings();
        else
            EnableAdvancedBuildings();

        if (!structureObj)
            return;

        if (structure.currentlyBuilding) {
            buildTimer -= Time.deltaTime * GameManager.population;
            if (buildTimer <= 0)
                BuildFinished();

            return;
        }

        if (CheckValidLocation())
            structureObj.GetComponent<SpriteRenderer>().material.color = Color.green;
        else
            structureObj.GetComponent<SpriteRenderer>().material.color = Color.red;

        if (Input.GetMouseButtonDown(0))
        {
            if (CheckValidLocation())
            {
                StartBuilding();
            }
            else
            {
                audioPlayer.clip = sounds[2];
                audioPlayer.Play();
                // TODO: display error message
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            buildingSelected = false;
            Destroy(structureObj);
        }
    }
}
