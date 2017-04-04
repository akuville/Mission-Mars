using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFloor : Level5Controller {


    private Rigidbody2D rb;
    private float speed = 3;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        string nam = collision.gameObject.name;
        if (nam == "lefttop")
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        if (nam == "topleft")
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (nam == "topright")
        {
            rb.velocity = new Vector2(rb.velocity.x, speed * -1);
        }

        if (nam == "righttop")
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (nam == "rightbottom")
        {
            rb.velocity = new Vector2(speed * -1, rb.velocity.y);
        }

        if (nam == "bottomright")
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (nam == "bottomleft")
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }

        if (nam == "leftbottom")
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            transform.position = new Vector3(9.7f, transform.position.y);
        }

    }
}
