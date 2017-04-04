using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpSpeed;
    public float jetpackSpeed;
    public GameObject jetpackParticles;
    public GameObject bullet;
    public GameObject gunMuzzle;
    
    private Animator anim;
    private Rigidbody2D hero;
    private GameObject particlesClone;

    private bool onGround = false;
    private bool facingRight = true;
    private bool hittingWall = false;
    private bool dead = false;
    private bool hasKey = false;
    private bool hasJetpack = false;
    private bool jetpackOn = false;
    private bool hasGun = false;
    private bool shootingCooldownOn = false;

    private float move;
    private float jetPush;
    private int state;
    private float platformpush = 0;
    private float gravitypush = 0;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        hero = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (dead == false)
        {
            //getting move direction and flipping
            move = Input.GetAxis("Horizontal");
            if (move > 0 && facingRight == false) FlipHero(); else if (move < 0 && facingRight == true) FlipHero();

            //jumping with Up Arrow key, also using jetpack
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (hasJetpack)
                {
                    jetpackOn = true;
                    if (particlesClone != null)
                    {
                        DestroyImmediate(particlesClone);
                    }
                    particlesClone = Instantiate(jetpackParticles);
                }else if (onGround)
                {
                    //normal jump
                    hero.AddForce(new Vector2(0, jumpSpeed));
                }
                
                onGround = false;
            }

            //Up Arrow key is released
            if (Input.GetKeyUp(KeyCode.UpArrow) && hasJetpack)
            {
                jetpackOn = false;
                jetPush = 0;
                Destroy(particlesClone, 0.5f);
            }

            //checking if moving and setting animation
            if (hero.velocity.x != 0)
            {
                if (hero.velocity.x == platformpush)
                {
                    state = 0;
                }
                else
                {
                    state = 1;
                }
                
            }
            else
            {
                state = 0;
            }

            if (onGround == false)
            {
                state = 2;
            }

            //shooting with Space key
            if (Input.GetKeyDown(KeyCode.Space) && hasGun && shootingCooldownOn == false)
            {
                if (state == 0) state = 4; else if (state == 1) state = 5;

                float shootDir;
                float bxPos;
                Quaternion rot;

                if (facingRight)
                {
                    shootDir = 1;
                    bxPos = transform.position.x + 1;
                    rot = Quaternion.identity;
                }
                else
                {
                    shootDir = -1;
                    bxPos = transform.position.x - 1;
                    rot = new Quaternion(0, 0, 1, 0);
                }
                //create the muzzle effect and make it a child of the player so it follows the player
                GameObject muzzClone = Instantiate(gunMuzzle, new Vector3(bxPos, transform.position.y, 0), rot);
                muzzClone.transform.parent = transform;
                Destroy(muzzClone, 0.5f);

                //create bullet and launch it, and invoke a cooldown period
                GameObject bulletClone = Instantiate(bullet, new Vector3(bxPos, transform.position.y, 0), rot);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000 * shootDir, 0));
                Destroy(bulletClone, 1f); //bullet has timed life
                shootingCooldownOn = true;
                Invoke("GunCooldown", 1);  //shooting cooldown, a time before player can shoot again
                
            }


            //set animation after everything has been checked previously
            anim.SetInteger("State", state);
        }
        

    }

    void FixedUpdate()
    {
        if (dead == false)
        {
            //player can move left and right if not hitting a wall
            if (hittingWall == false)
            {
                hero.velocity = new Vector2(move * speed + platformpush, hero.velocity.y + gravitypush);
            }

            //jetpack stuff
            if (jetpackOn)
            {
                if (jetPush < jetpackSpeed)
                {
                    jetPush += 0.2f;
                }else if (jetPush > jetpackSpeed)
                {
                    jetPush = jetpackSpeed;
                }

                hero.velocity = new Vector2(hero.velocity.x, jetPush);
            }

            //particles need to follow for even after stopping to jetpack
            if (particlesClone != null)
            {
                Vector3 pdir = new Vector3();
                if (facingRight)
                {
                    pdir = new Vector3(transform.position.x - 0.3f, transform.position.y);
                }
                else
                {
                    pdir = new Vector3(transform.position.x + 0.3f, transform.position.y);
                }
                particlesClone.transform.position = pdir;
            }
        }
        
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //print("Hit with: " + collision.gameObject.tag);
        //collision with the floor
        if (collision.gameObject.tag == "Floor")
        {
            onGround = true;
        }

        //collision with the wall
        if (collision.gameObject.tag == "Wall")
        {
            hittingWall = true;
        }

        //collision with the spike and spikeball
        if (collision.gameObject.tag == "Spike")
        {
            dead = true;
            hero.velocity *= 0;
            anim.SetInteger("State", 3);
            Invoke("ResetGame", 2);
        }

        //collision with enemy
        if (collision.gameObject.tag == "Enemy")
        {
            //checking if the enemy is dead from its animation state, you cant die if hitting a dead enemy
            if (collision.gameObject.GetComponent<Animator>().GetInteger("State") != 1)
            {
                dead = true;
                hero.velocity *= 0;
                anim.SetInteger("State", 3);
                Invoke("ResetGame", 2);
            }
        }

        //collision with the gate
        if (collision.gameObject.tag == "Gate" && hasKey)
        {
            Destroy(collision.gameObject, 0.1f);
        }

    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        //collision with the door
        if (collision.gameObject.tag == "Door")
        {
            string scenename = SceneManager.GetActiveScene().name;

            if (scenename == "Level1")
            {
                SceneManager.LoadScene("Level2");
            }
            else if (scenename == "Level2")
            {
                SceneManager.LoadScene("Level3");
            }
            else if (scenename == "Level3")
            {
                SceneManager.LoadScene("Level4");
            }
            else if (scenename == "Level4")
            {
                SceneManager.LoadScene("Level5");
            }
        }

        //collision with the key icon
        if (collision.gameObject.tag == "Key")
        {
            Destroy(collision.gameObject, 0.1f);
            hasKey = true;
        }

        //collision with the jetpack icon
        if (collision.gameObject.tag == "Jetpack")
        {
            Destroy(collision.gameObject, 0.1f);
            hasJetpack = true;
        }

        //collision with the bullet ICON
        if (collision.gameObject.tag == "BulletIcon")
        {
            Destroy(collision.gameObject, 0.1f);
            hasGun = true;
        }
        //collision with gravityfield green
        if (collision.gameObject.tag == "Gravityfield_green")
        {
            gravitypush = 0.8f;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Gravityfield_green")
        {
            gravitypush = 0;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        string na = collision.gameObject.name;
        if (na == "Platformfloor1" || na == "Platformfloor2" || na == "Platformfloor3" || na == "Platformfloor4")
        {
            platformpush = 0;
        }

        if (collision.gameObject.tag == "Floor")
        {
            onGround = false;
        }

        if (collision.gameObject.tag == "Wall")
        {
            hittingWall = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        string na = collision.gameObject.name;
        GameObject go = collision.gameObject;
        
        if (na == "Platformfloor1" || na == "Platformfloor2" || na == "Platformfloor3" || na == "Platformfloor4")
        {
            
            float vel = go.GetComponent<Rigidbody2D>().velocity.x;
            
            if (vel != 0)
            {
                platformpush = vel;
            }
            else
            {
                platformpush = 0;
            }
        }
    }

    private void FlipHero()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        //so he doesnt get stuck in the wall (a bug fix)
        hittingWall = false;
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GunCooldown()
    {
        shootingCooldownOn = false;
    }
}
