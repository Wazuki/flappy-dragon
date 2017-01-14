using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    Rigidbody2D myRigidBody;
    float dragonXPosition;
    bool isScoreAdded;

    GameManager gameManager;

	// Use this for initialization
	void Start ()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
        myRigidBody.velocity = new Vector2(-2.5f, 0); //Handles velocity in the x direction

        dragonXPosition = GameObject.Find("Dragon").transform.position.x;

        isScoreAdded = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update ()
    {
		if(transform.position.x <= dragonXPosition)
        {
            if(!isScoreAdded)
            {
                AddScore();
                isScoreAdded = true;
                Debug.Log("Scored by " + gameObject.name);
            }
        }

        if(transform.position.x <= -10f)
        {
            Destroy(gameObject);
        }
	}


    void AddScore()
    {
        gameManager.GmAddScore();
    }
}
