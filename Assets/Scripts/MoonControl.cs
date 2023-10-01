using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoonControl : MonoBehaviour
{
    Collider2D col;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (col.bounds.Contains(other.bounds.max) && col.bounds.Contains(other.bounds.min))
            SelectBuildLocation.insideMoon = true;
        else
            SelectBuildLocation.insideMoon = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
            transform.rotation *= Quaternion.Euler(Input.GetAxis("Mouse X") * new Vector3(0,0,-1));
    }
}
