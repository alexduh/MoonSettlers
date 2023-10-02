using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = true;
    public static bool paused = false;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject moon;
    [SerializeField] private BuildBehavior buildMenu;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject introCanvas;

    float dayTimer;
    float dayDuration = 30f;
    public static int dayNumber;
    public static int population;

    float food;
    float water;
    float oxygen;

    private float foodDeathTimer;
    private float waterDeathTimer;
    [SerializeField] private float oxygenDeathTimer;
    private float foodTimeLimit = 15;
    private float waterTimeLimit = 10;
    private float oxygenTimeLimit = 5;
    private bool spawnedFood = false;
    private bool spawnedWater = false;
    private bool spawnedOxygen = false;

    GameObject foodWarning;
    GameObject waterWarning;
    GameObject oxygenWarning;

    [SerializeField] private GameObject emergencyTextPrefab;
    [SerializeField] private Transform frontCanvas;
    [SerializeField] private TMP_Text dayText;

    [SerializeField] private TMP_Text foodText;
    [SerializeField] private TMP_Text waterText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private Slider oxygenBar;

    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private GameObject shipSpawn;
    GameObject ship;

    public static Dictionary<string, int> buildingDict;

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void BackToMain()
    {
        mainCanvas.SetActive(false);
        introCanvas.SetActive(true);
        gameOverObj.SetActive(false);
    }

    public void StartGame()
    {
        mainCanvas.SetActive(true);
        introCanvas.SetActive(false);
        gameOverObj.SetActive(false);
        gameOver = false;

        dayTimer = 0;
        dayNumber = 0;
        population = 0;

        food = 5;
        water = 5;
        oxygen = 25;

        foodDeathTimer = -1;
        waterDeathTimer = -1;
        oxygenDeathTimer = -1;

        ClearSettlers();
        ClearMoon();
        buildMenu.EnableBuilding();

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

        Time.timeScale = 1;
    }

    private void ClearSettlers()
    {
        foreach (GameObject settler in GameObject.FindGameObjectsWithTag("Settler"))
            Destroy(settler);
    }

    private void ClearMoon()
    {
        foreach (Transform child in moon.transform)
            Destroy(child.gameObject);
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

        if (population == 0 && dayNumber > 1)
            GameOver();
    }

    void CalculateResources()
    {
        GainResources();
        DrainResources();
    }

    void GainResources()
    {
        food += 10 * buildingDict["Greenhouse"] * Time.deltaTime / dayDuration;
        water += 35 * buildingDict["SynthesisReactor"] * Time.deltaTime / dayDuration;
        oxygen += 35 * buildingDict["Greenhouse"] * Time.deltaTime / dayDuration;

        if (oxygen > oxygenBar.maxValue)
            oxygen = oxygenBar.maxValue;
    }

    void DrainResources()
    {
        food -= Mathf.Pow(.9f, population * buildingDict["Headquarters"]) * population * Time.deltaTime / dayDuration;
        water -= Mathf.Pow(.95f, population * buildingDict["Headquarters"]) * (20 * buildingDict["Greenhouse"] + population) * Time.deltaTime / dayDuration;
        oxygen -= (15 * buildingDict["SynthesisReactor"] + population) * Time.deltaTime / dayDuration;

        if (FoodborneDisease.diseased)
            DiseaseEffect();
    }

    void DiseaseEffect()
    {
        if (buildingDict["Hospital"] > 0)
        {
            GameObject diseaseCured = Instantiate(emergencyTextPrefab, frontCanvas);
            diseaseCured.GetComponent<TMP_Text>().text = "Pathogen exterminated, food supply secured!";
            FoodborneDisease.diseased = false;
        }

        if (food <= 0)
            FoodborneDisease.diseased = false;
        else
            food -= 2f * Time.deltaTime; // TODO: balance the disease food loss value!
    }

    void UpdateInfo()
    {
        if (food <= 0)
        {
            food = 0;

            if (!spawnedFood)
            {
                foodWarning = Instantiate(emergencyTextPrefab, frontCanvas);
                foodWarning.GetComponent<TMP_Text>().text = "the settlers are gnawing at their fingers...";
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
                waterWarning = Instantiate(emergencyTextPrefab, frontCanvas);
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
                oxygenWarning = Instantiate(emergencyTextPrefab, frontCanvas);
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
    }

    void UpdateTime()
    {
        if (foodDeathTimer > 0)
            foodDeathTimer -= Time.deltaTime;
        else if (foodDeathTimer > -1)
        {
            StartCoroutine(SettlerDeath());
            foodDeathTimer = foodTimeLimit;
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
        {
            StartCoroutine(SettlerDeath());
            oxygenDeathTimer = oxygenTimeLimit;
        }

        dayTimer -= Time.deltaTime;
        if (dayTimer < 0)
        {
            if (dayNumber == 30)
                GameOver(); // player has won!

            dayTimer = dayDuration;
            dayNumber++;
            StartCoroutine(DisplayDay(dayNumber));
            if (dayNumber % 3 == 1)
                SpawnShip();

        }
    }

    void SpawnShip()
    {
        ship = Instantiate(shipPrefab, shipSpawn.transform.position, Quaternion.identity);

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
        if (BuildBehavior.structureObj)
            Destroy(BuildBehavior.structureObj);

        gameOver = true;
        DismantleBehavior.dismantleMode = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        Time.timeScale = 0;
        gameOverObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
            return;

        CheckPause();
        GetPopulation();
        CalculateResources();
        UpdateInfo();
        UpdateTime();
    }
}
