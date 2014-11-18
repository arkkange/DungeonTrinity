using UnityEngine;
using System.Collections;

public class BarFallowCamera : MonoBehaviour {

    [SerializeField]
    Transform _myCamera;

	// Use this for initialization
	void Start () {
        this.transform.rotation = _myCamera.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.rotation = _myCamera.transform.rotation;
	}
}
