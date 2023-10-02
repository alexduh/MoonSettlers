using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeteorShower : SurvivalThreat
{
    [SerializeField] private ParticleSystem meteorParticleSystem;
    [SerializeField] private GameObject buildingDestroyedSound;

    public override void EventWarning()
    {
        threatWarning = Instantiate(emergencyTextPrefab, frontCanvas);
        threatWarning.GetComponent<TMP_Text>().text = "WARNING: Meteor shower incoming";
    }

    public override void StartEvent()
    {
        meteorParticleSystem.Play();
        // TODO: play meteor shower sound effect!
        StartCoroutine(TriggerMeteorShower());
    }

    IEnumerator TriggerMeteorShower()
    {
        FireTurrets();
        yield return new WaitWhile(() => meteorParticleSystem.isPlaying);
        if (GameManager.buildingDict["DefenseTurret"] == 0)
        {
            foreach (Transform child in moon.transform)
            {
                if (child.gameObject.tag != "Hospital" && child.gameObject.tag != "Beacon" && child.gameObject.tag != "DefenseTurret")
                {
                    Instantiate(buildingDestroyedSound);
                    GameManager.buildingDict[child.gameObject.tag]--;
                    Destroy(child.gameObject);
                }
            }
        }
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
