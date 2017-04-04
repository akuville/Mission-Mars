using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        //print("Hit: " + collision.gameObject.name);

        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Animator>().SetInteger("State", 1); //player its death animation
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false; //disable its collider so it doesnt hit the hero
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f; //make its gravity 0 so it doesnt drop through the floor because its collider is disabled
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); //make it stop moving when dead
            collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - 1.1f, 0); //a fix to the bad death animation in the sprite, dont use normally
            Destroy(collision.gameObject, 1f);
        }

        //if hitting anything except the hero or the enemymarker then destroy the bullet
        if (collision.gameObject.tag != "Hero" && collision.gameObject.tag != "EnemyMarker") Destroy(gameObject);
    }
}
