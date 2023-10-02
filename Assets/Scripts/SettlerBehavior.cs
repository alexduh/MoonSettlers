using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettlerBehavior : MonoBehaviour
{
    float speed = 1;
    private GameObject moon;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Settler")
        {
            int direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:
                    transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime);
                    break;
                case 1:
                    transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y);
                    break;
                case 2:
                    transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime);
                    break;
                case 3:
                    transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moon = GameObject.FindWithTag("Moon");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.PingPong(Time.time, .5f) - .25f));

        Structure s;
        foreach(Transform child in moon.transform)
        {
            s = child.gameObject.GetComponent<Structure>();
            if (s.currentlyBuilding)
            {
                transform.position = Vector2.Lerp(transform.position, s.transform.position, 2);
                // TODO: move towards buildLocation Lerp not working!
            }
        }
    }
}
