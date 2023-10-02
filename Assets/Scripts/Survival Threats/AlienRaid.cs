using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlienRaid : SurvivalThreat
{
    public override void EventWarning()
    {
        threatWarning = Instantiate(emergencyTextPrefab, frontCanvas);
        threatWarning.GetComponent<TMP_Text>().text = "WARNING: Alien raid imminent";
    }

    public override void StartEvent()
    {
        throw new System.NotImplementedException();
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
