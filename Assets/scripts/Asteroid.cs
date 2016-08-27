using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float width;
    public float height;
    public float rotationSpeed = 2.0f;

    private Rigidbody2D rb;
    private float xForce;
    private float yForce;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        setPosition();
        setInitialMovement();
    }

    void setPosition() {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        transform.position = new Vector2( Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight));
    }

    void setInitialMovement() {
        rb.AddTorque(0.5f, ForceMode2D.Impulse);
        xForce = Random.Range(-4, 4) * (Time.deltaTime + rotationSpeed);
        yForce = Random.Range(-4, 4) * (Time.deltaTime + rotationSpeed);
        rb.AddForce(new Vector2(xForce, yForce));
    }

    void move() {
        // TODO: Add code to change asteroids direction
    }

    // Update is called once per frame
    void Update () {
        checkBoundaries();
        move();
	}

 
    void checkBoundaries()
    {

        Vector3 pos = transform.position;
        // es 6 en total, va de -3 a 3
        float verticalSeen = Camera.main.orthographicSize * 2.0f;
        // es 8 en total, va desde -4 a 4
        float horizontalSeen = verticalSeen * Screen.width / Screen.height;

        float maxX = horizontalSeen / 2;
        float minX = maxX * -1;
        float maxY = verticalSeen / 2;
        float minY = maxX * -1;

        if (pos.x < minX - width)
        {
            transform.position = new Vector3(maxX, pos.y, pos.z);
        }
        if (pos.x > maxX + width)
        {
            transform.position = new Vector3(minX, pos.y, pos.z);
        }
        if (pos.y < minY - height)
        {
            transform.position = new Vector3(pos.x, maxY, pos.z);
        }
        if (pos.y > maxY + height)
        {
            transform.position = new Vector3(pos.x, minY, pos.z);
        }
    }
}
