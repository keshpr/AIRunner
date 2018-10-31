using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallController : MonoBehaviour {

    public GameObject fireball;
    public Transform mouth;

    public int initialHealth;
    public float bounceSpeed = 5;
    public int phaseTime = 20;
    public float fireBallSpeed = 5;
    public float fireLag = 2;
    public float amp = 2;
    public float freq = 10;
    public float smallVal = 0.1f;
    public int numShotsPhase = 10;

    private int health;
    private int phase;
    private int moveDir;
    private Rigidbody2D rb;
    private float fireTime;
    private float t;
    private int phaseFlag;
    private int movedFlag;
	// Use this for initialization
	void Start () {
        t = 0;
        phaseFlag = 0;
        movedFlag = 0;
        fireTime = 0;
        health = initialHealth;
        moveDir = 1;
        rb = GetComponent<Rigidbody2D>();
        phase = 1;
	}

    private void Update()
    {
        if (health <= 0)
            Die();
        if (phase == 1)
        {
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
            }
            bounceHor();
            if (fireTime >= fireLag)
            {                
                shootPhaseOne();
                fireTime = 0;
            }
        }
        else if (phase == 2)
        {
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
            }
            sineWave();
            if (fireTime >= fireLag)
            {
                shootPhaseTwo();
                fireTime = 0;
            }
        }
        else if (phase == 3)
        {
            if(t >= phaseTime)
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
            }
            moveToCentre();
            if (fireTime >= fireLag && movedFlag == 1)
            {
                float angle = 0;
                shootPhaseThree(angle);
                fireTime = 0;
            }
        }
        fireTime += Time.deltaTime;
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "One")
            health--;
        if (collision.gameObject.tag == "Wall")
        {
            if (moveDir == 1)
                moveDir = -1;
            else
                moveDir = 1;
            incrementPhase();
        }
        
    }

    private void incrementPhase()
    {
        if (t >= phaseTime && phase == 3)
            phaseFlag = 1;
        else if (t >= phaseTime)
        {
            phase++;
            phaseFlag = 1;
        }
    }
    private void Die(){
        //TODO: Animation of death
        Destroy(this.gameObject);
    }

    private void Shoot(Vector2 dir)
    {
        GameObject f = Instantiate(fireball,mouth.position,Quaternion.identity);
        f.GetComponent<Rigidbody2D>().velocity = dir.normalized * fireBallSpeed;
    }

    private void bounceHor()
    {
        t += Time.deltaTime;
        rb.velocity = new Vector2(moveDir * bounceSpeed,0);
    }
    private void shootPhaseOne()
    {
        Shoot(Vector2.down);
    }
    private void sineWave()
    {
        t += Time.deltaTime;
        rb.velocity = new Vector2(moveDir * bounceSpeed, -amp * Mathf.Cos(t * freq));
    }
    private void shootPhaseTwo()
    {
        Shoot(new Vector2(Mathf.Sin(Mathf.PI / 4), - Mathf.Cos(Mathf.PI/4)));
        Shoot(new Vector2(-Mathf.Sin(Mathf.PI / 4), -Mathf.Cos(Mathf.PI / 4)));
    }
    private void moveToCentre()
    {
        t += Time.deltaTime;
        if (Mathf.Abs(transform.position.x) > smallVal)
            rb.velocity = new Vector2(moveDir * bounceSpeed, 0);
        else
        {
            rb.velocity = new Vector2(0, 0);
            movedFlag = 1;
        }
    }
    private void shootPhaseThree(float angle)
    {
        for (int i = 0; i < numShotsPhase; i++)
        {
            Shoot(new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle)));
            angle += 2*Mathf.PI / numShotsPhase;
        }
    }
    private void shootPhaseFour()
    {
        float angle = - Mathf.PI / 2;
        for (int i = 0; i < numShotsPhase; i++)
        {
            Shoot(new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle)));
            angle += Mathf.PI / numShotsPhase;
        }
        for (int i = 0; i < numShotsPhase; i++)
        {
            Shoot(new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle)));
            angle -= Mathf.PI / numShotsPhase;
        }
    }
}
