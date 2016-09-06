using UnityEngine;
using System.Collections;

public class LargeAsteroid : Asteroid {

    public override void resetAsteroid() {
		setPosition ();
		base.setInitialMovement ();

	}

	void setPosition() {
		float screenWidth = Screen.width * 0.8f;
		float screenHeight = Screen.height * 0.8f;
		transform.position = new Vector2( Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight));
	}

}
