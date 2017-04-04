using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Controller : MonoBehaviour {

    public GameObject[] platforms;

    private float startingspeed = 3;
    //private float xdif;
    private float ydif;
    // Use this for initialization
    void Start ()
    {
        //xdif = platforms[0].transform.position.x - platforms[1].transform.position.x;
        ydif = platforms[0].transform.position.y - platforms[1].transform.position.y;
        //print("xdif: " + xdif + " ydif: " + ydif);
        platforms[1].GetComponent<Rigidbody2D>().velocity = new Vector2(0, startingspeed);
        platforms[3].GetComponent<Rigidbody2D>().velocity = new Vector2(startingspeed, 0);
        platforms[5].GetComponent<Rigidbody2D>().velocity = new Vector2(0, startingspeed * -1);
        platforms[7].GetComponent<Rigidbody2D>().velocity = new Vector2(startingspeed * -1, 0);

        //platforms[1].GetComponent<Rigidbody2D>().velocity = new Vector2(0, startingspeed);
    }
	
	// Update is called once per frame
	void Update ()
    {
        platforms[0].transform.position = new Vector3(platforms[1].transform.position.x, platforms[1].transform.position.y + ydif);
        platforms[2].transform.position = new Vector3(platforms[3].transform.position.x, platforms[3].transform.position.y + ydif);
        platforms[4].transform.position = new Vector3(platforms[5].transform.position.x, platforms[5].transform.position.y + ydif);
        platforms[6].transform.position = new Vector3(platforms[7].transform.position.x, platforms[7].transform.position.y + ydif);
    }

    public void hitMarker(string hitname)
    {

    }
}
