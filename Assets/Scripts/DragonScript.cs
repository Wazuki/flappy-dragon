using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonScript : MonoBehaviour {

    public bool isAlive;

    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private float jumpForce;

    private GameManager gameManagerScript;

    private AudioSource dragonFlapSound;

    void Start()
    {
        isAlive = true;

        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myAnimator = gameObject.GetComponent<Animator>();

        jumpForce = 10f;
        myRigidBody.gravityScale = 4f;
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        dragonFlapSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(isAlive && gameManagerScript.gameHasStarted)
        {
            if(Input.GetMouseButton(0))
            {
                Flap();
            }
            CheckIfDragonVisibleOnScreen();
        }
        else
        {
            myRigidBody.velocity = new Vector2(0f, 0f);
            myRigidBody.angularVelocity = 0f;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        }

    }

    void Flap()
    {
        myRigidBody.velocity = new Vector2(0, jumpForce);

        myAnimator.SetTrigger("Flap");
        if(!dragonFlapSound.isPlaying) dragonFlapSound.Play();
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if(target.gameObject.tag == "Obstacles")
        {
            isAlive = false;
        }
    }

    void CheckIfDragonVisibleOnScreen()
    {
        if(Mathf.Abs(gameObject.transform.position.y) > 5.3)
        {
            isAlive = false;
        }
    }
}
