using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool paused = false;
    [SerializeField] private GameObject pause;

    float dayTimer;
    float dayDuration = 30f;
    int dayNumber;
    public static int population;

    float food;
    float water;
    float oxygen;

    [SerializeField] private float foodDeathTimer = -1;
    [SerializeField] private float waterDeathTimer = -1; // TODO: remove [SerializeField]
    private float oxygenDeathTimer = -1;
    private float foodTimeLimit = 30;
    private float waterTimeLimit = 10;
    private float oxygenTimeLimit = 5;
    private bool spawnedFood = false;
    private bool spawnedWater = false;
    private bool spawnedOxygen = false;

    GameObject foodWarning;
    GameObject waterWarning;
    GameObject oxygenWarning;

    [SerializeField] private GameObject emergencyTextPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private TMP_Text dayText;

    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private Slider oxygenBar;

    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject shipSpawn;

    public static Dictionary<string, int> buildingDict;

    // Start is called before the first frame update
    void Start()
    {
        dayNumber = 0;
        population = 0;

        food = 5; 
        water = 6;
        oxygen = 500;
        // TODO: change/balance start values

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

        if (population == 0)
            GameOver();
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
        food -= Mathf.Pow(.95f, population * buildingDict["Headquarters"]) * population * Time.deltaTime / dayDuration;
        water -= Mathf.Pow(.95f, population * buildingDict["Headquarters"]) * (buildingDict["Greenhouse"] + population) * Time.deltaTime / dayDuration;
        oxygen -= (buildingDict["SynthesisReactor"] + population) * Time.deltaTime / dayDuration;
        // TODO: balance the numbers!
    }

    void UpdateInfo()
    {
        if (food <= 0)
        {
            food = 0;

            if (!spawnedFood)
            {
                foodWarning = Instantiate(emergencyTextPrefab, canvasTransform);
                foodWarning.GetComponent<TMP_Text>().text = "the settlers are gnawing on their fingers...";
                foodDeathTimer = foodTimeLimit;
                spawnedFood = true;
            }
        }
        else
        {
            foodDeathTimer = -1;
            if (spawnedFood)
                spawnedFood = false;
        }

        if (water <= 0)
        {
            water = 0;

            if (!spawnedWater)
            {
                waterWarning = Instantiate(emergencyTextPrefab, canvasTransform);
                waterWarning.GetComponent<TMP_Text>().text = "the water supply has dried up...";
                waterDeathTimer = waterTimeLimit;
                spawnedWater = true;
            }
        }
        else
        {
            waterDeathTimer = -1;
            if (spawnedWater)
                spawnedWater = false;
        }

        if (oxygen <= 0)
        {
            oxygen = 0;

            if (!spawnedOxygen)
            {
                oxygenWarning = Instantiate(emergencyTextPrefab, canvasTransform);
                oxygenWarning.GetComponent<TMP_Text>().text = "the crew's holding their breath...";
                oxygenDeathTimer = oxygenTimeLimit;
                spawnedOxygen = true;
            }
        }
        else
        {
            oxygenDeathTimer = -1;
            if (spawnedOxygen)
                spawnedOxygen = false;
        }

        foodText.text = food.ToString("F1") + " kg";
        waterText.text = water.ToString("F1") + " L";
        oxygenBar.value = oxygen;
        // TODO: make oxygen bar change!
    }

    void UpdateTime()
    {
        if (foodDeathTimer > 0)
            foodDeathTimer -= Time.deltaTime;
        else if (foodDeathTimer > -1)
        {
            StartCoroutine(SettlerDeath());
            food = 10;
            foodDeathTimer = -1;
        }

        if (waterDeathTimer > 0)
            waterDeathTimer -= Time.deltaTime;
        else if (waterDeathTimer > -1)
        {
            StartCoroutine(SettlerDeath());
            waterDeathTimer = waterTimeLimit;
        }

        if (oxygenDeathTimer > 0)
            oxygenDeathTimer -= Time.deltaTime;
        else if (oxygenDeathTimer > -1)
            StartCoroutine(SettlerDeath());

        dayTimer -= Time.deltaTime;
        if (dayTimer < 0)
        {
            dayTimer = dayDuration;
            dayNumber++;
            StartCoroutine(DisplayDay(dayNumber));
            if (dayNumber % 3 == 1)
                SpawnShip();

        }
    }

    void SpawnShip()
    {
        Instantiate(shipPrefab, shipSpawn.transform.position, Quaternion.identity);

        // TODO: settlers slowly float towards moon on spawn
    }

    IEnumerator SettlerDeath()
    {
        GameObject dyingSettler = GameObject.FindWithTag("Settler");
        dyingSettler.GetComponent<AudioSource>().Play();
        
        yield return new WaitWhile(() => dyingSettler.GetComponent<AudioSource>().isPlaying);
        Destroy(dyingSettler);
        // TODO: add settler death animation (fall over and fade away)
    }

    void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    void TogglePause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            pause.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pause.SetActive(true);
        }

        paused = !paused;
    }

    void GameOver()
    {
        // TODO: freeze time, disable pausing and interacting with anything, show statistics and retry button
    }

    // Update is called once per frame
    void Update()
    {
        CheckPause();
        GetPopulation();
        CalculateResources();
        UpdateInfo();
        UpdateTime();
    }
}
