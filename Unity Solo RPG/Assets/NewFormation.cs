using UnityEngine;
using System.Collections;

public class NewFormation : MonoBehaviour {

	public Vector3 startLocation;



	public GameObject[] Enemys;
	public Vector3[] StartPositions;
	public Vector3[] Positions;
	public float speed;

	// Use this for initialization
	void Start () {
		startLocation = transform.position;

		//Enemys = GameObject.FindGameObjectsWithTag ("Enemy");

		/*Vector3 vect1 = new Vector3 (0, 0, 10);
		Vector3 vect2 = new Vector3 (0, 0, 0);
		Vector3 vect3 = new Vector3 (-10, 0, 0);
		Vector3 vect4 = new Vector3 (10, 0, 0);*/

		var quat = transform.rotation;

		//StartPositions.Initialize ();
		/*StartPositions [0] =   startLocation - (quat*vect1) ;
		StartPositions [1] =   startLocation - (quat*vect2) ;
		StartPositions [2] =   startLocation - (quat*vect3) ;
		StartPositions [3] =   startLocation - (quat*vect4) ;*/

		/*StartPositions [0] =   Quaternion.FromToRotation(startLocation, new Vector3 (-10, 0, 0)) ;
		StartPositions [1] =   Quaternion.FromToRotation(startLocation, new Vector3 (10, 0, -10)) ;
		StartPositions [2] =   Quaternion.FromToRotation(startLocation, new Vector3 (10, 0, 0)) ;
		StartPositions [3] =   Quaternion.FromToRotation(startLocation, new Vector3 (10, 0, -10)) ;*/

		for (int i = 0; i < Enemys.Length; i++) {
			Positions[i] = startLocation - (quat*StartPositions[i]);
			Enemys[i].GetComponent<BasicIa>().startLocation = Positions[i];
		}
	}
	
	// Update is called once per frame
	void Update () {
		checkMovementFormation ();
		formedMobsPosition (transform.position);
		checkMovementMobs ();
	}

	void checkMovementFormation()
	{
		var distance = Vector3.Distance (transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
		Vector3 PlayerIsHere =  GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position;
		var distanceFromStart = Vector3.Distance (transform.position, startLocation);
		Vector3 point = startLocation - transform.position;

		var rotate = Quaternion.LookRotation (GameObject.FindGameObjectWithTag ("Player").transform.position - transform.position).eulerAngles;
		rotate.z = 0;
		rotate.x = 0;


		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(rotate), Time.deltaTime * 2.0f);
		
		if (distance < 35) {
			PlayerIsHere.y = 0;
			rigidbody.velocity = PlayerIsHere.normalized * speed;
		} else if (distance > 4) {
			if (distanceFromStart > 1) {
				point.y = 0;
				rigidbody.velocity = point.normalized * speed;
			} else {
				rigidbody.velocity = new Vector3();
			}
		}
	}

	void formedMobsPosition(Vector3 newStartPos)
	{
		/*Vector3 vect1 = new Vector3 (0, 0, 10);
		Vector3 vect2 = new Vector3 (0, 0, 0);
		Vector3 vect3 = new Vector3 (-10, 0, 0);
		Vector3 vect4 = new Vector3 (10, 0, 0);*/

		var quat = transform.rotation;

		/*StartPositions [0] =   newStartPos - (quat*vect1) ;
		StartPositions [1] =   newStartPos - (quat*vect2) ;
		StartPositions [2] =   newStartPos - (quat*vect3) ;
		StartPositions [3] =   newStartPos - (quat*vect4) ;*/

		/*StartPositions [0] =   Quaternion.FromToRotation(newStartPos, new Vector3 (-10, 0, 0)) ;
		StartPositions [1] =   Quaternion.FromToRotation(newStartPos, new Vector3 (10, 0, -10)) ;
		StartPositions [2] =   Quaternion.FromToRotation(newStartPos, new Vector3 (10, 0, 0)) ;
		StartPositions [3] =   Quaternion.FromToRotation(newStartPos, new Vector3 (10, 0, -10)) ;*/

		for(int i = 0; i < Positions.Length; i++) {
			Positions [i] = newStartPos - (quat*StartPositions[i]);
				}
		}

	void checkMovementMobs() {

		for (int i = 0; i < Enemys.Length; i++) {
			BasicIa bIa = Enemys[i].GetComponent<BasicIa>();
			bIa.startLocation = Positions[i];
				}
		}

	enum StancesFormation {
		follow,
		attack,
		contourn,
		flee
	}
}
