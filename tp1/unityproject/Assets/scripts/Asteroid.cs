﻿using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float width;
    public float height;
    public float rotationSpeed = 2.0f;
    public int score;

    private float xForce;
    private float yForce;
    private float nextMovementChange;
    private float timeDelta;

    protected void Start () {
        timeDelta = 10f;
        nextMovementChange = Time.time + timeDelta;
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
        if(Time.time > nextMovementChange) {
            nextMovementChange = Time.time + timeDelta;
            xForce = Random.Range(0.0f, 1.0f) < 0.5f? Random.Range(-2.0f, -1.0f): Random.Range(1.0f, 2.0f);
            yForce = Random.Range(0.0f, 1.0f) < 0.5f ? Random.Range(-2.0f, -1.0f) : Random.Range(1.0f, 2.0f);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
        }
	}

    void checkBoundaries() {
        Vector3 pos = transform.position;
        float maxX = 6.6f;
        float minX = -6.6f;
        float minY = -3.3f;
        float maxY = 3.3f;

        if (pos.x < minX) {
            transform.position = new Vector3(maxX, pos.y, pos.z);
        }
        if (pos.x > maxX) {
            transform.position = new Vector3(minX, pos.y, pos.z);
        }
        if (pos.y < minY) {
            transform.position = new Vector3(pos.x, maxY, pos.z);
        }
        if (pos.y > maxY) {
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
			GameController.instance.addScore(score);
            GameController.instance.getAsteroidExplosion(transform.position);
            Destroy(gameObject);
            GameController.instance.destroyAsteroid();
        } else if (gameObject.CompareTag("mediumAsteroid")) {
            GameController.instance.addScore(score);
            GameController.instance.getAsteroidExplosion(transform.position);
            Destroy(gameObject);
            GameController.instance.destroyAsteroid();
        }

	}
}
