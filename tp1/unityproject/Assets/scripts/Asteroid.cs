using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float width;
    public float height;
    public float rotationSpeed = 2.0f;
    public int score;

    private float xForce;
    private float yForce;

    protected void Start () {
		resetAsteroid ();
    }

	public virtual void resetAsteroid() {
		
	}

	public void setPosition(float x, float y) {
		transform.position = new Vector2(x, y);
	}

    protected void setInitialMovement() {
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		GetComponent<Rigidbody2D> ().angularVelocity = 0.0f;
		GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
        xForce = Random.Range(-4, 4) * (Time.deltaTime + rotationSpeed);
        yForce = Random.Range(-4, 4) * (Time.deltaTime + rotationSpeed);
		GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
    }

    void Update () {
        checkBoundaries();
	}
		
    void checkBoundaries() {

        Vector3 pos = transform.position;
        // es 6 en total, va de -3 a 3
        float verticalSeen = Camera.main.orthographicSize * 2.0f;
        // es 8 en total, va desde -4 a 4
        float horizontalSeen = verticalSeen * Screen.width / Screen.height;

        float maxX = horizontalSeen / 2;
        float minX = maxX * -1;
        float maxY = verticalSeen / 2;
        float minY = maxX * -1;

        if (pos.x < minX - width) {
            transform.position = new Vector3(maxX, pos.y, pos.z);
        }
        if (pos.x > maxX + width) {
            transform.position = new Vector3(minX, pos.y, pos.z);
        }
        if (pos.y < minY - height) {
            transform.position = new Vector3(pos.x, maxY, pos.z);
        }
        if (pos.y > maxY + height) {
            transform.position = new Vector3(pos.x, minY, pos.z);
        }
    }

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.CompareTag("bolt")) {
			collider.gameObject.GetComponent<Bolt> ().returnBolt ();
		}
			
		float prevPositionX = gameObject.transform.position.x;
		float prevPositionY = gameObject.transform.position.y;
		if (gameObject.CompareTag("largeAsteroid")) {
			GameController.instance.getMediumAsteroid (prevPositionX, prevPositionY);
			GameController.instance.returnAsteroid(gameObject);
            GameController.instance.addScore(score);
            GameController.instance.getAsteroidExplosion(transform.position);
		} else if (gameObject.CompareTag("mediumAsteroid")) {
			GameController.instance.returnMediumAsteroid(gameObject);
            GameController.instance.addScore(score);
            GameController.instance.getAsteroidExplosion(transform.position);
        }

	}
}
