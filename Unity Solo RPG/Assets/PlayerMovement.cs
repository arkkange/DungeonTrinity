using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 50;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float horizontalSpeed = Input.GetAxis ("Horizontal") * moveSpeed;
		float verticalSpeed = Input.GetAxis ("Vertical") * moveSpeed;

		rigidbody.velocity = new Vector3 (horizontalSpeed, rigidbody.velocity.y, verticalSpeed);

	}
}
