using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehavior : MonoBehaviour
{
    public GameObject settlerPrefab;
    bool settlersSpawned = false;
    float shipSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!settlersSpawned && transform.position.x > 0)
        {
            for (int i = 0; i < 2 + GameManager.buildingDict["Beacon"]; i++)
                Instantiate(settlerPrefab, transform.position, Quaternion.identity);

            settlersSpawned = true;
        }

        if (transform.position.x <= 11)
            transform.position += new Vector3(shipSpeed, 0) * Time.deltaTime;
        else
            Destroy(gameObject);
    }
}
