using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public int lives;
    public int score;
    public int level;
    public GameObject asteroidPrefab;
    public int asteroidPoolSize;
    
    private float nextActionTime;
    public float period;
    private Stack<GameObject> inactiveAsteroidsStack;

    // pool de tiros
    // pool de Asteroides


    // Use this for initialization
    void Start () {
        this.lives = 3;
        this.score = 0;
        this.level = 1;
        this.nextActionTime = 0.0f;
        this.period = 1.0f;
        createAsteroidPool();
	}

    public void createAsteroidPool() {
        inactiveAsteroidsStack = new Stack<GameObject>();
        for (int i = 0; i < this.asteroidPoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(asteroidPrefab);
            obj.SetActive(false);
            inactiveAsteroidsStack.Push(obj);
        }
    }

    public GameObject getAsteroid() {
        if (inactiveAsteroidsStack.Count > 0 ) {
            return inactiveAsteroidsStack.Pop();
        }
        return null;
    }

    public void returnAsteroid(GameObject asteroid) {
        inactiveAsteroidsStack.Push(asteroid);
    }

    // Update is called once per frame
    void Update () {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            GameObject obj = getAsteroid();
            if (obj != null)
            {
                obj.active = true;
            }
        }
    }
}
