using UnityEngine;
using System.Collections;

public class LargeAsteroid : Asteroid {

    public override void resetAsteroid() {
		setPosition ();
		base.setInitialMovement ();

	}

	void setPosition() {
        float x = 0.0f;
        float y = 0.0f;
        x = Random.Range(0, 2) == 0 ? Random.Range(-20, -8): Random.Range(8, 20);
        y = Random.Range(0, 2) == 0 ? Random.Range(-20, -5) : Random.Range(5, 20);
        float screenWidth = 13.2f * 0.8f;
		float screenHeight = 6.6f * 0.8f;
		transform.position = new Vector2( x, y);
	}
}
