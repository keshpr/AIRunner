using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour {

    public float speed;
    public int maxHealth;
    public int hitMoveAmount;
    private int health;
    private Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0,-speed);
	}
	
	// Update is called once per frame
	void Update () {
        //move each item down at a constant rate
        if (health <= 0)
            DestroyObstacle();
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
            Debug.Log(("Collided with something"));
            if (collision.gameObject.tag == "One")
            {
                health -= collision.gameObject.GetComponent<BulletScript>().ReturnDamage();
                Destroy(collision.gameObject);
            }
            // if zero hits, it is destroyed without affecting the obstacle
            if (collision.gameObject.tag == "Zero")
                Destroy(collision.gameObject);
        if (collision.gameObject.tag == "Player")
        {
            Vector2 diff = new Vector2(transform.position.x - collision.transform.position.x, -speed);
            rb.AddForce(diff * hitMoveAmount, ForceMode2D.Impulse);
        }
        }
    private void DestroyObstacle() {
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
