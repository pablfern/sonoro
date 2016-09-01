using UnityEngine;
using System.Collections;

public class LargeAsteroid : Asteroid {

	public override void resetAsteroid() {
		setPosition ();
		base.setInitialMovement ();

	}

	void setPosition() {
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		transform.position = new Vector2( Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight));
	}
}
