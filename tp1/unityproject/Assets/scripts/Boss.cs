using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

    //public SpriteRenderer fireSprite;
    public float rotationSpeed = 10.0f;
    public float drag = 2.0f;
    public float width;
    public float height;

    public GameObject playerExplosion;
    public GameObject enemyBolt;
    private GameObject explosion;

    private Rigidbody2D rb;
    //public Rigidbody2D bolt;
    public float boltSpeed = 10.0f;
    public AudioSource boltAudio;
    public AudioSource collisionAudio;
    private bool doFlash = false;
    private bool waiting = false;
    private float flashWait = 0.125f;

    private float xForce;
    private float yForce;
    private int life;
    private float nextBolt;
    private float nextBoltTime;
    private float nextSpecial;
    private float nextSpecialTime;
    private float nextMovementChange;
    private float timeDelta;

    void Start()
    {
        //fireSprite.enabled = false;
        explosion = null;
        life = 20;
        nextBolt = Random.Range(0.3f, 0.5f);
        nextBoltTime = Time.time + nextBolt;
        nextBolt = Random.Range(0.5f, 1f);
        nextSpecial = Time.time + nextSpecial;
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<Renderer>().bounds.size.x;
        height = GetComponent<Renderer>().bounds.size.y;
        resetBlueEnemy();
        timeDelta = 2f;
        nextMovementChange = Time.time + timeDelta;
    }

    void Update() {
        checkBoundaries();
        shoot();
        specialShoot();
        if (Time.time > nextMovementChange) {
            nextMovementChange = Time.time + timeDelta;
            xForce = Random.Range(0.0f, 1.0f) < 0.5f ? Random.Range(-10.0f, -5.0f) : Random.Range(5.0f, 10.0f);
            yForce = Random.Range(0.0f, 1.0f) < 0.5f ? Random.Range(-10.0f, -5.0f) : Random.Range(5.0f, 10.0f);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
        }
    }

    public void specialShoot() {
        if (Time.time > nextSpecialTime) {
            nextSpecialTime = Time.time + nextSpecial;
            GameObject bolt;
            for (int i = 0; i < 4; i++) {
                bolt = (GameObject)Instantiate(enemyBolt);
                bolt.transform.position = transform.position;
                bolt.transform.rotation = transform.rotation;
                bolt.transform.Rotate(0.0f, 90.0f*i, 0.0f);
                bolt.GetComponent<Bolt>().setCreationTime();
                bolt.GetComponent<Bolt>().setBoltTTL(2.0f);
                boltAudio.Play();
            }
        }
    }

    public void shoot() {
        if (Time.time > nextBoltTime) {
            nextBoltTime = Time.time + nextBolt;
            GameObject bolt = (GameObject)Instantiate(enemyBolt);
            bolt.transform.position = transform.position;
            bolt.transform.rotation = transform.rotation;
            bolt.GetComponent<Bolt>().setCreationTime();
            bolt.GetComponent<Bolt>().setBoltTTL(2.0f);
            boltAudio.Play();
        }
    }

    public void resetBlueEnemy() {
        setPosition();
        setInitialMovement();
    }

    void setPosition() {
        transform.position = new Vector3(0.0f, 7.0f);
    }

    void setInitialMovement() {
        float torque = Random.Range(0.1f, 1f);
        GetComponent<Rigidbody2D>().AddTorque(torque, ForceMode2D.Impulse);
        yForce = Random.Range(3, 7) * (Time.deltaTime + rotationSpeed * 3);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, yForce));
    }


    public void enableFlash(bool doFlash) {
        this.doFlash = doFlash;
    }

    void flash() {
        if (this.doFlash) {
            StartCoroutine(FlashWait(flashWait));
        }
    }

    public void restartPosition() {
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0, 0);
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
            collider.gameObject.GetComponent<Bolt>().returnBolt();
        }
        if (collider.CompareTag("bolt")) {
            collisionAudio.Play();
            life = life - 1;
            Debug.Log(life);
            if (life <= 0)
            {
                explosion = (GameObject)Instantiate(playerExplosion, transform.position, new Quaternion());
                explosion.transform.position = transform.position;
                explosion.gameObject.GetComponent<ParticleSystem>().time = 0;
                explosion.gameObject.GetComponent<ParticleSystem>().Play();
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator FlashWait(float duration)
    {
        this.waiting = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(duration);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(duration);
        this.waiting = false;
    }
}
