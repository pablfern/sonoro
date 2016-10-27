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

    void Start()
    {
        //fireSprite.enabled = false;
        explosion = null;
        life = 20;
        nextBolt = Random.Range(0.5f, 1f);
        nextBoltTime = Time.time + nextBolt;
        rb = GetComponent<Rigidbody2D>();
        width = GetComponent<Renderer>().bounds.size.x;
        height = GetComponent<Renderer>().bounds.size.y;
        resetBlueEnemy();
    }

    void Update()
    {
        checkBoundaries();
        shoot();
    }

    public void shoot()
    {
        if (Time.time > nextBoltTime)
        {
            nextBoltTime = Time.time + nextBolt;
            GameObject bolt = (GameObject)Instantiate(enemyBolt);
            bolt.transform.position = transform.position;
            bolt.transform.rotation = transform.rotation;
            bolt.GetComponent<Bolt>().setCreationTime();
            boltAudio.Play();
        }
    }

    public void resetBlueEnemy()
    {
        setPosition();
        setInitialMovement();
    }

    void setPosition()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        transform.position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(5.0f, 6.0f), 0);
    }

    void setInitialMovement()
    {
        float torque = Random.Range(0.1f, 1f);
        GetComponent<Rigidbody2D>().AddTorque(torque, ForceMode2D.Impulse);
        yForce = Random.Range(3, 7) * (Time.deltaTime + rotationSpeed * 3);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, yForce));
    }


    public void enableFlash(bool doFlash)
    {
        this.doFlash = doFlash;
    }

    void flash()
    {
        if (this.doFlash)
        {
            StartCoroutine(FlashWait(flashWait));
        }
    }

    /*
    void slowDown()
    {
        Vector2 startVelocity = rb.velocity;
        rb.velocity = Vector2.Lerp(startVelocity, new Vector2(0, 0), Time.deltaTime * drag);
    }*/
    /*
    void checkInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z + Time.deltaTime + rotationSpeed));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z - (Time.deltaTime + rotationSpeed)));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            float angle = transform.rotation.eulerAngles.z + 90;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);
            rb.AddForce(new Vector2(x * (Time.deltaTime + rotationSpeed), y * (Time.deltaTime + rotationSpeed)));

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bolt = GameController.instance.getBolt();
            bolt.transform.position = transform.position;
            bolt.transform.rotation = transform.rotation;
            bolt.GetComponent<Bolt>().setCreationTime();
            boltAudio.Play();
        }
    }*/


    public void restartPosition()
    {
        // TODO: Reset rotation
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0, 0);
    }

    void checkBoundaries()
    {

        Vector3 pos = transform.position;
        // es 6 en total, va de -3 a 3
        float verticalSeen = Camera.main.orthographicSize * 2.0f;
        // es 8 en total, va desde -4 a 4
        float horizontalSeen = verticalSeen * Screen.width / Screen.height;

        float maxX = 6.7f;
        float minX = -6.7f;
        float minY = -3.4f;
        float maxY = 3.4f;

        if (pos.x < minX)
        {
            transform.position = new Vector3(maxX, pos.y, pos.z);
        }
        if (pos.x > maxX)
        {
            transform.position = new Vector3(minX, pos.y, pos.z);
        }
        if (pos.y < minY)
        {
            transform.position = new Vector3(pos.x, maxY, pos.z);
        }
        if (pos.y > maxY)
        {
            transform.position = new Vector3(pos.x, minY, pos.z);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Enemy Collision");
        if (collider.CompareTag("bolt"))
        {
            collider.gameObject.GetComponent<Bolt>().returnBolt();
        }
        if (collider.CompareTag("bolt") || collider.CompareTag("Player"))
        {
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

        /*
        if (explosion == null)
        {
            explosion = (GameObject)Instantiate(playerExplosion, transform.position, new Quaternion());
        }
        else
        {
            explosion.transform.position = transform.position;
            explosion.gameObject.GetComponent<ParticleSystem>().time = 0;
            explosion.gameObject.GetComponent<ParticleSystem>().Play();
        }
        */
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
