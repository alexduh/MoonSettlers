using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmergencyTextBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        float yStart = Random.Range(-200, 200);
        transform.localPosition = new Vector3(1500, yStart, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.paused)
            return;

        if (transform.localPosition.x > -1500)
            transform.Translate(Vector3.left / 2);
        else
            Destroy(gameObject);
    }
}
