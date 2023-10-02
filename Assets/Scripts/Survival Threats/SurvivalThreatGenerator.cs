using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalThreatGenerator : MonoBehaviour
{
    float threatTimer;
    SurvivalThreat threatType;

    [SerializeField] protected FoodborneDisease diseaseObj;
    [SerializeField] protected MeteorShower meteorShowerObj;
    [SerializeField] protected AlienRaid alienRaidObj;

    IEnumerator GenerateThreat(SurvivalThreat threat)
    {
        threat.EventWarning();
        yield return new WaitForSeconds(10);
        threat.StartEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        threatTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (threatTimer <= 0 && (GameManager.dayNumber == 5 || GameManager.dayNumber == 7))
        {
            threatTimer = 10f;
            threatType = diseaseObj;
        }

        if (threatTimer <= 0 && (GameManager.dayNumber == 9 || GameManager.dayNumber == 10))
        {
            threatTimer = 10f;
            threatType = meteorShowerObj;
        }

        if (threatTimer > 0)
        {
            threatTimer -= Time.deltaTime;
            if (threatTimer < 0)
                StartCoroutine(GenerateThreat(threatType));
        }
    }
}
