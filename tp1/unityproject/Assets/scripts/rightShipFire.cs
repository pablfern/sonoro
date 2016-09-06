using UnityEngine;
using System.Collections;

public class rightShipFire : MonoBehaviour {

    private float endFire = 0.0f;

    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            endFire = Time.time + 0.2f;
            gameObject.GetComponent<Renderer>().enabled = true;
        }
        if (endFire <= Time.time)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
