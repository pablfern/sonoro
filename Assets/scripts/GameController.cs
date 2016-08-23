using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public int lives;
    public int score;
    public int level;

    // pool de tiros
    // pool de Asteroides

    
	// Use this for initialization
	void Start () {
        this.lives = 3;
        this.score = 0;
        this.level = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
