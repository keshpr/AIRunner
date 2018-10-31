using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    
    public GameObject zero, one;
    public ParticleSystem rightParticle, leftParticle;
    public GameObject heart;

    public float tiltFactor, shootTime;
    public float touchThreshold = 0.1f;
    public int hitScore;
    public int maxHealth = 5;
    public float rightLocHeart = 7.5f;
    public float leftLocHeart;
    public float heartGap = 1.2f;
    public float heartY = -2;
    //public float hitMoveAmount;


    private Vector2 startTouch;
    private Vector2 touchDirection;
    private GameController gameController;
    private Camera cam;
    private float touchTime;
    private bool directionChosen;
    private Rigidbody2D rb;
    private float timeSinceLastShot = 0;
    private int health;
    private int leftOrRight;
    private bool heartsPrinted;
    private GameObject[] hearts;

    // Use this for initialization
    void Start () {
        touchTime = 0;
        health = maxHealth;
        heartsPrinted = false;
        //Creates a reference to the GameController object
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
            Debug.Log("Found the game controller");
        }
        else
            Debug.Log("FAILED TO FIND GAME CONTROLLER");
        //start the particle systems to begin
        rightParticle.Stop();
        leftParticle.Stop();

        cam = Camera.main;

        rb = GetComponent<Rigidbody2D>();

        hearts = new GameObject[maxHealth];

	}
	
	// Update is called once per frame
	void Update () {
        //updates every 
        if (health <= 0)
        {
            removeHearts();
            gameController.KillPlayer();
        }
        rb.velocity = new Vector2((rb.velocity.x * 0.15f)+(Input.acceleration.x * Time.deltaTime*tiltFactor), 0);

        if (Input.GetButtonDown("Fire1") && CanShootAgain()) {
            Vector3 p = Input.mousePosition;
            //check if the pause button is hit
            if(cam.ScreenToWorldPoint(p).x > -8 || cam.ScreenToWorldPoint(p).y < 6)
                Shoot();
        }
        timeSinceLastShot += Time.deltaTime;
        
        

	}

    public void printHearts()
    {
        float locHeart;
        float gap;
        if (leftOrRight == 0)
        {
            locHeart = leftLocHeart;
            gap = -heartGap;
        }
        else
        {
            locHeart = rightLocHeart;
            gap = heartGap;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i] = Instantiate(heart, new Vector3(locHeart - i * gap, heartY, 0), Quaternion.identity);
        }
        heartsPrinted = true;
    }

    private void removeHeart()
    {
        Destroy(hearts[health]);
    }

    public void removeHearts()
    {
        if (!heartsPrinted)
            return;
        for (int i = 0; i < health; i++)
        {
            Destroy(hearts[i]);
        }
        heartsPrinted = false;
    }

    public void LoseHealthToGameController(){
        
    }

    //checks if the gun has cooled down
    private bool CanShootAgain(){
        return (timeSinceLastShot > shootTime);
            
    }

    private void OnCollisionExit2D(Collision2D other){
        //stops the particles when the wall is no longer being collided with
        if(other.gameObject.tag == "Wall"){
            rightParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            leftParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)  {
       /* if (collision.gameObject.tag == "Wall")
            Handheld.Vibrate();*/
    }


    private void OnCollisionEnter2D(Collision2D other) {

        Debug.Log("Inside collision");
        
        //deals with sparking when the walls are hit
        if(other.gameObject.tag == "Wall"){
            Handheld.Vibrate();
            if (this.transform.position.x < 0)
                leftParticle.Play();
            else
                rightParticle.Play();
        }
        
    }
    public void TakeDamage()
    {
        Debug.Log("Taking Damage");
        health -= 1;
        removeHeart();
    }

    void GameOver(bool win){
        gameController.GameOver(win);
    }

    public int returnHealth()
    {
        return health;
    }

    //shoots either a 1 or 0 at the enemy
    public void Shoot()
    {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            Instantiate(zero, this.transform.position+(0.75f* Vector3.up), Quaternion.identity);
            if (rand == 1)
            Instantiate(one, this.transform.position+(0.75f*Vector3.up), Quaternion.identity);
        timeSinceLastShot = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fireball")
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }
    public void moveRight()
    {
        leftOrRight = 1;
    }

    public void moveLeft()
    {
        leftOrRight = 0;
    }

}
