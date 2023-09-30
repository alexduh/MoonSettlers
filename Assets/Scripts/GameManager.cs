using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float dayTimer;
    float dayDuration = 30f;
    int dayNumber;

    float food;
    float water;
    int population;
    float oxygen;

    [SerializeField] private TMP_Text dayText;

    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text oxygenText;

    private Dictionary<string, int> buildingDict;

    // Start is called before the first frame update
    void Start()
    {
        dayNumber = 0;
        buildingDict = new Dictionary<string, int> 
        {
            { "Headquarters", 0},
            { "Greenhouse", 0},
            { "Beacon", 0},
            { "ResearchLab", 0},
            { "SynthesisReactor", 0},
            { "Hospital", 0},
            { "DefenseTurret", 0}
        };
    }

    public IEnumerator FadeTextToFullAlpha(float t, TMP_Text name)
    {
        name.color = new Color(name.color.r, name.color.g, name.color.b, 0);
        while (name.color.a < 1.0f)
        {
            name.color = new Color(name.color.r, name.color.g, name.color.b, name.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TMP_Text name)
    {
        name.color = new Color(name.color.r, name.color.g, name.color.b, 1);
        while (name.color.a > 0.0f)
        {
            name.color = new Color(name.color.r, name.color.g, name.color.b, name.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    IEnumerator DisplayDay(int number)
    {
        dayText.text = "Day " + number.ToString();
        yield return StartCoroutine(FadeTextToFullAlpha(1, dayText));
        yield return new WaitForSeconds(5);
        yield return StartCoroutine(FadeTextToZeroAlpha(1, dayText));
    }

    void GetPopulation()
    {
        GameObject[] settlers = GameObject.FindGameObjectsWithTag("Settler");
        population = settlers.Length;
        populationText.text = population.ToString();
    }

    void CalculateResources()
    {
        GainResources();
        DrainResources();
    }

    void GainResources()
    {
        food += buildingDict["Greenhouse"] * Time.deltaTime;
        water += buildingDict["SynthesisReactor"] * Time.deltaTime;
        oxygen += buildingDict["Greenhouse"] * Time.deltaTime;
        // TODO: balance the numbers!
    }

    void DrainResources()
    {
        food -= population * Time.deltaTime;
        water -= (buildingDict["Greenhouse"] + population) * Time.deltaTime;
        oxygen -= (buildingDict["SynthesisReactor"] + population) * Time.deltaTime;
        // TODO: use Headquarters in calculation and balance the numbers!
    }

    void UpdateTime()
    {
        dayTimer -= Time.deltaTime;
        if (dayTimer < 0)
        {
            dayTimer = dayDuration;
            dayNumber++;
            StartCoroutine(DisplayDay(dayNumber));
            if (dayNumber % 2 == 1)
            {
                // TODO: spawn ship with settlers inside
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetPopulation();
        CalculateResources();
        UpdateTime();
    }
}
