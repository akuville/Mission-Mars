using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private float movementSpeed = 3;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        //only move if not dead, check if dead from the animation state
        if (GetComponent<Animator>().GetInteger("State") != 1)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(movementSpeed, 0);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //hitting a marker set on the level which indicates to change the movement direction for the enemy
        if (collision.gameObject.tag == "EnemyMarker")
        {
            //change direction
            movementSpeed *= -1;

            //flip the gameobject
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
