using UnityEngine;
using System.Collections;

public class BasicIa : MonoBehaviour {

	public Vector3 startLocation;

	public float speed = 2.0f;
	//bool aggresion = false;


	// Use this for initialization
	void Start () {
		startLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		checkMovement ();
	}

	void checkMovement()
	{
		float distanceFromStart = Vector3.Distance (transform.position, startLocation);
		Vector3 point = startLocation - transform.position;

		var distance = Vector3.Distance (transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
		Vector3 PlayerIsHere =  GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position;

		if (distance < 20) {
			PlayerIsHere.y = 0;
			rigidbody.velocity = PlayerIsHere.normalized * speed;
				} else
		if (distanceFromStart > 0.2f) {
			point.y = 0;
			rigidbody.velocity = point.normalized * speed;
		} else {
			rigidbody.velocity = new Vector3();
		}
	}

	enum Stances {
		attack,
		block,
		flee,
		waitInFormation
	}
}
