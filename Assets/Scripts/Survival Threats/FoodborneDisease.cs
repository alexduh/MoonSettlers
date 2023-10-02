using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodborneDisease : SurvivalThreat
{
    public static bool diseased = false;

    public override void EventWarning()
    {
        threatWarning = Instantiate(emergencyTextPrefab, frontCanvas);
        if (GameManager.buildingDict["Hospital"] == 0)
            threatWarning.GetComponent<TMP_Text>().text = "something has gotten into the food supply...";
    }

    public override void StartEvent()
    {
        diseased = true;
    }

}
