using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text missionStatusText;
    [SerializeField] private TMP_Text dayNumberText;
    [SerializeField] private TMP_Text populationText;
    

    private void OnEnable()
    {
        if (GameManager.population > 0)
        {
            missionStatusText.text = "Mission: Success!";
            populationText.text = "Population: " + GameManager.population;
            populationText.gameObject.SetActive(true);
        }
        else
        {
            missionStatusText.text = "Mission: Failure...";
            populationText.gameObject.SetActive(false);
        }

        dayNumberText.text = "Day: " + GameManager.dayNumber;
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
