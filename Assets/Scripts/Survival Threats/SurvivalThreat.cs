using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class SurvivalThreat : MonoBehaviour
{
    [SerializeField] protected GameObject emergencyTextPrefab;
    [SerializeField] protected Transform frontCanvas;
    [SerializeField] protected GameObject moon;

    protected GameObject threatWarning;

    public abstract void EventWarning();

    public abstract void StartEvent();

    protected void FireTurrets()
    {
        foreach (Transform child in moon.transform)
        {
            if (child.gameObject.tag == "DefenseTurret")
            {
                // TODO: DefenseTurrets start firing
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
