using UnityEngine;
using System.Collections;

public class buttonTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	debug.log("test");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ButtonTest(string str)
    {
        Debug.Log(str);
    }
}
