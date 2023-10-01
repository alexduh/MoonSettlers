using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBuildLocation : MonoBehaviour
{
    public static bool insideMoon;
    bool overlappingOther = false;
    GameObject structure;
    [SerializeField] Material wireframe;
    [SerializeField] Material solid;

    public void SelectStructure(GameObject selected)
    {
        structure = Instantiate(selected);
        structure.GetComponent<SpriteRenderer>().material = wireframe;
        insideMoon = false;
    }

    bool CheckValidLocation()
    {
        return insideMoon && !overlappingOther;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!structure)
            return;

        if (CheckValidLocation())
            structure.GetComponent<SpriteRenderer>().material.color = Color.green;
        else
            structure.GetComponent<SpriteRenderer>().material.color = Color.red;

        if (Input.GetMouseButtonDown(0))
        {
            if (CheckValidLocation())
            {
                Debug.Log("Selected valid build site, need to implement StartBuilding()");
                //StartBuilding();
            }
            else
            {
                
                // TODO: play error sound effect, display error message
            }
        }
        if (Input.GetMouseButtonDown(1))
            Destroy(structure);
    }
}
