using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb;
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
        string hitname = collision.gameObject.name;

        if (hitname == "lefttop")
        {
            rb.velocity = new Vector2(speed / 2, speed);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void test()
    {
        print("test succesfull");
    }
}
