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
        threatTimer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (threatTimer > 0)
        {
            if (threatTimer == 30)
                StartCoroutine(GenerateThreat(threatType));

            threatTimer -= Time.deltaTime;
            return;
        }

        if (GameManager.dayNumber == 5 || GameManager.dayNumber == 7)
        {
            threatTimer = 30;
            threatType = diseaseObj;
        }

        if (GameManager.dayNumber == 9 || GameManager.dayNumber == 11)
        {
            threatTimer = 30;
            threatType = meteorShowerObj;
        }

        if (GameManager.dayNumber >= 12)
        {
            threatTimer = 30;
            int randEvent = Random.Range(0, 2);
            switch (randEvent)
            {
                case 0:
                    threatType = diseaseObj;
                    break;

                case 1:
                    threatType = meteorShowerObj;
                    break;

                default:
                    Debug.Log("This survival event not implemented yet");
                    break;

            }
            
        }

        
    }
}
