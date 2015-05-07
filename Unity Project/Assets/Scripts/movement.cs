using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.A))
						transform.position += Vector3.left * Time.deltaTime;
		if (Input.GetKey (KeyCode.D))
						transform.position += Vector3.right * Time.deltaTime;
	

	}



}

