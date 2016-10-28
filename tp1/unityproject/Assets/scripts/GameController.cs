using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController instance;

    public int lives;
    public int score;
    public int highScore;

    public GameObject largeAsteroidPrefab;
	public GameObject mediumAsteroidPrefab;
    public GameObject asteroidExplosionPrefab;
    public GameObject blueEnemyPrefab;
    public GameObject bossPrefab;

	public GameObject starPrefab;
	public GameObject boltPrefab;
    public GameObject spaceShip;
    public GameObject startText;
    public GameObject scoreText;
    public GameObject highScoreText;
    public GameObject livesText;
    public GameObject levelText;
    public GameObject gameOverText;
    public GameObject messageText;

    public int asteroidPoolSize;
	public int mediumAsteroidPoolSize;
	public int starPoolSize;
	public int boltPoolSize;
    
    public AudioSource startMusic;
	public AudioSource gameOverMusic;
    public AudioSource level1Music;
    public AudioSource level2Music;
    public AudioSource level3Music;
    public AudioSource computerVoice1;
    public AudioSource computerVoice2;
    public AudioSource computerVoice3;

    private float nextActionTime;
	private float nextStarActionTime;
    public float period;
	public float starPeriod;
    private bool gameStarted = false;
    private int level;
    private int asteroidsDestroyed;
    private int blueEnemiesDestroyed;
    private bool initLevel;
    private int levelState;

    private List<GameObject> largeAsteroidList;
	private List<GameObject> mediumAsteroidList;
	private List<GameObject> starList;
	private List<GameObject> boltList;
    private List<GameObject> blueEnemyList;

    // Use this for initialization
    void Start () {
       // startMusic.Play();
        this.nextActionTime = 0.0f;
		this.nextStarActionTime = 0.0f;
        this.period = 3.0f;
		this.starPeriod = 1.0f;
        this.level = 1;
	}

    private void createBlueEnemyPool() {
        blueEnemyList = new List<GameObject>();
        for(int i = 0; i < 3; i++) {
            GameObject obj = (GameObject)Instantiate(blueEnemyPrefab);
            obj.SetActive(false);
            blueEnemyList.Add(obj);
        }
    }

    public void getAsteroidExplosion(Vector3 position) {
        GameObject obj = (GameObject)Instantiate(asteroidExplosionPrefab);
        obj.transform.position = position;
        obj.gameObject.GetComponent<ParticleSystem>().time = 0;
        obj.gameObject.GetComponent<ParticleSystem>().Play();
        obj.SetActive(true);
        StartCoroutine(DelayedDestroyObject(obj, 5));
    }

    IEnumerator DelayedDestroyObject(GameObject obj, float seconds) {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
        Destroy(obj);
        
    }

    private void createMediumAsteroidPool() {
		mediumAsteroidList = new List<GameObject> ();
		for (int i = 0 ; i < mediumAsteroidPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(mediumAsteroidPrefab);
			obj.SetActive(false);
			mediumAsteroidList.Add(obj);
		}
	}

	private void createBoltPool() {
		boltList = new List<GameObject> ();
		for (int i = 0 ; i < boltPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(boltPrefab);
			obj.SetActive(false);
			boltList.Add(obj);
		}
	}

    public void createAsteroidPool() {
		largeAsteroidList = new List<GameObject> ();
        for (int i = 0 ; i < this.asteroidPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(largeAsteroidPrefab);
			obj.SetActive(false);
			largeAsteroidList.Add(obj);
		}
    }
		
	public void createStarPool() {
		starList = new List<GameObject>();
        for (int i = 0 ; i < this.starPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(starPrefab);
			obj.SetActive (false);
			starList.Add(obj);
		}
	}

	public GameObject getMediumAsteroid(float x, float y) {
		if (mediumAsteroidList.Count > 0) {
			GameObject obj = mediumAsteroidList[0];
			obj.SetActive (true);
			mediumAsteroidList.RemoveAt (0);
			obj.GetComponent<Asteroid> ().setPosition(x, y);
			obj.GetComponent<MediumAsteroid> ().resetAsteroid ();
			return obj;
		}
		return null;
	}

	public void returnMediumAsteroid(GameObject mediumAsteroid) {
		mediumAsteroid.SetActive (false);
		mediumAsteroidList.Add (mediumAsteroid);
	}

	public GameObject getBolt() {
		if (boltList.Count > 0) {
			GameObject obj = boltList[0];
			obj.SetActive (true);
			boltList.RemoveAt (0);
			return obj;
		}
		return null;
	}

	public void returnBolt(GameObject bolt) {
		bolt.SetActive (false);
		boltList.Add (bolt);
	}

    public GameObject getAsteroid() {
        if (largeAsteroidList.Count > 0 ) {
			GameObject obj = largeAsteroidList[0];
			largeAsteroidList.RemoveAt(0);
            return obj;
        }
        return null;
    }
		
	public void returnAsteroid(GameObject asteroid) {
		largeAsteroidList.Add(asteroid);
		asteroid.SetActive(false);
	}

	public GameObject getStar() {
		if (starList.Count > 0) {
			GameObject obj = starList[0];
			starList.RemoveAt(0);
			return obj;
		}
		return null;
	}

    public GameObject getBlueEnemy() {
        if(blueEnemyList.Count > 0) {
            GameObject obj = blueEnemyList[0];
            blueEnemyList.RemoveAt(0);
            return obj;
        }
        return null;
    }

	public void returnStar(GameObject star) {
		star.SetActive(false);
		starList.Add (star);
	}

    public void destroyAsteroid() {
        this.asteroidsDestroyed = 1 + this.asteroidsDestroyed;
    }

    void checkInput() {
        if (Input.GetKey(KeyCode.Return)) {
            startGame();
        }
    }

    void destroyBlueEnemies() {
        for(int i = 0; i < 4; i++) {
            GameObject obj = GameObject.FindWithTag("BlueEnemy") as GameObject;
            if(obj == null) {
                return;
            }
            obj.SetActive(false);
            Destroy(obj);
        }
    }

    void startGame() {
        gameStarted = true;
        this.lives = 3;
        this.score = 0;
		this.nextActionTime = Time.time;
		this.nextStarActionTime = Time.time;
        this.period = 3.0f;
        this.starPeriod = 1.0f;
        this.level = 1;
        this.levelState = 0;
        this.initLevel = true;
        this.asteroidsDestroyed = 0;
        this.blueEnemiesDestroyed = 0;
        createAsteroidPool();
        createBlueEnemyPool();
        createStarPool();
        createBoltPool();
        createMediumAsteroidPool();
        destroyBlueEnemies();
        spaceShip.gameObject.GetComponent<SpaceShip>().restartPosition();
        gameOverText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        spaceShip.gameObject.SetActive(true);
        if(highScore>0) {
            highScoreText.gameObject.SetActive(true);
        } else {
            highScoreText.gameObject.SetActive(false);
        }
    }

    void refreshUITexts() {
        scoreText.GetComponent<UnityEngine.UI.Text>().text = "Score: " + score.ToString();
        livesText.GetComponent<UnityEngine.UI.Text>().text = "Lives: " + lives.ToString();
        highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High Score: " + highScore.ToString();
        levelText.GetComponent<UnityEngine.UI.Text>().text = "Level: " + level;
    }

    void spawnAsteroid() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            GameObject obj = getAsteroid();
            if (obj != null) {
                obj.SetActive(true);
  				obj.GetComponent<LargeAsteroid> ().resetAsteroid();
            }
        }
    }

	void spawnStars() {
		if (Time.time > nextStarActionTime) {
			nextStarActionTime += starPeriod;
			GameObject obj = getStar();
			if (obj != null) {
				obj.SetActive (true);
				obj.GetComponent<Star> ().resetStar ();
			}
		}
	}

    void spawnBlueEnemy() {
        GameObject obj = getBlueEnemy();
        if(obj != null) {
            obj.SetActive(true);
            obj.GetComponent<BlueEnemy>().resetBlueEnemy();
        }
    }

    void spawnBoss() {
        GameObject obj = (GameObject)Instantiate(bossPrefab);
        obj.SetActive(true);
    }

    void removeEnemies() { 
        if (GameObject.FindGameObjectsWithTag("BlueEnemy").Length > 0) {
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("BlueEnemy")) {
                obj.SetActive(true);
                Destroy(obj);
            }
        }
        if (GameObject.FindGameObjectsWithTag("Boss").Length > 0) {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Boss")) {
                obj.SetActive(true);
                Destroy(obj);
            }
        }
    }

    void removeAsteroids() {
		Destroy (GameObject.FindWithTag("largeAsteroid"));
		Destroy (GameObject.FindWithTag("mediumAsteroid"));
		createAsteroidPool ();
		createMediumAsteroidPool ();
		this.nextActionTime = Time.time;
		this.nextStarActionTime = Time.time;
    }

   public void playerKilled() {
        lives -= 1;
        if(lives < 0) {
            gameOver();
        } else {
            spaceShip.gameObject.GetComponent<SpaceShip>().restartPosition();
            spaceShip.gameObject.SetActive(true);
            //removeAsteroids ();
			StartCoroutine(DestructionWait(5.0f));
        }
    }

    void winGame() {
        gameStarted = false;
        removeAsteroids();
        removeEnemies();
        if (score > highScore && score > 0) {
            highScore = score;
            highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High score: " + highScore.ToString();
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "You win!!\n Press ENTER to play again";
        } else {
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "You win!!\n Press ENTER to play again";
        }

        gameOverMusic.Play();
        spaceShip.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
    }

    void gameOver() {
		gameStarted = false;
		removeAsteroids();
        removeEnemies();
        if (score > highScore && score > 0) {
            highScore = score;
            highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High score: " + highScore.ToString();
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "Game Over!\n New high Score!\n" + score.ToString() + "\n Press ENTER to try again";
        } else {
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "Game Over!\n Press ENTER to try again";
        }
		gameOverMusic.Play ();
        spaceShip.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
    }

    void Update () {
        if (!gameStarted) { 
            checkInput();
			//removeAsteroids();
        } else {
            refreshUITexts();
            spawnStars();
            switch (level) {
                case 1:
                    level1Update();
                    break;
                case 2:
                    level2Update();
                    break;
                case 3:
                    level3Update();
                    break;
            }
        }
    }

    void level1Update() {
        fadeOutMusic(startMusic, 0.0f, 0.75f);
        fadeInMusic(level1Music, 0.6f, 0.03f);
        Debug.Log(this.levelState);
        switch (this.levelState) {
            case 0:
                StartCoroutine(ShowMessage("Level " + this.level, 3));
                computerVoice1.Play();
                level1Music.Play();
                StartCoroutine(changeLevelState(2, 8));
                this.levelState = 1;
                break;
            case 1:
                break;
            case 2:
                Debug.Log(string.Format("State: {0}", this.levelState));
                Debug.Log("Spawning Asteroid");
                spawnAsteroid();
                if (asteroidsDestroyed == 30) {
                    levelState = 3;
                }
                break;
            case 3:
                level = 2;
                levelState = 0;
                initLevel = true;
                break;
        }        
    }

    void level2Update() {
        if (initLevel) {
            StartCoroutine(ShowMessage("Level " + this.level, 3));
            initLevel = false;
        } else {
            spawnBlueEnemy();
            if(GameObject.FindGameObjectsWithTag("BlueEnemy").Length == 0) {
                level = 3;
                initLevel = true;
            }
        }
    }

    void level3Update() {
        if (initLevel) {
            StartCoroutine(ShowMessage("Level " + this.level, 3));
            initLevel = false;
            spawnBoss();
        } else {
            // Spawn Boss!
            // Check win condition!
            if (GameObject.FindGameObjectsWithTag("Boss").Length == 0) {
                winGame();
            }
        }
    }

    IEnumerator changeLevelState(int state,  float delay) {
        yield return new WaitForSeconds(delay);
        Debug.Log(string.Format("Changing level state to: {0}", state));
        this.levelState = state;
    }

    IEnumerator ShowMessage(string message, float delay) {
        messageText.GetComponent<UnityEngine.UI.Text>().text = message;
        messageText.GetComponent<UnityEngine.UI.Text>().enabled = true;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        messageText.GetComponent<UnityEngine.UI.Text>().enabled = false;
        messageText.gameObject.SetActive(false);
    }

    void Awake () {
		if (instance == null) {
			instance = this;
		}

		else if (instance != this) {
			Destroy(gameObject); 
		}
	}

    public void addScore(int score) {
        this.score += score;
    }

    private void fadeInMusic(AudioSource audio, float maxVolume, float step) {
        if (audio.volume < maxVolume) {
            audio.volume += step * Time.deltaTime;
        }
    }

    private void fadeOutMusic(AudioSource audio, float minVolume, float step) {
        if (audio.volume > minVolume) {
            audio.volume -= step * Time.deltaTime;
        }
    }


	IEnumerator DestructionWait(float duration) {
		spaceShip.gameObject.GetComponent<Collider2D> ().enabled = false;
		spaceShip.gameObject.GetComponent<SpaceShip> ().enableFlash (true);
		yield return new WaitForSeconds(duration);
		spaceShip.gameObject.GetComponent<Collider2D> ().enabled = true;
		spaceShip.gameObject.GetComponent<SpaceShip> ().enableFlash (false);
	}
}
