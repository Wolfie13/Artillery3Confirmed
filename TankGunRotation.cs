using UnityEngine;
using System.Collections;

public class TankGunRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("w")) {
			transform.Rotate(Vector3.forward,2,0);
		}
		if (Input.GetKey ("s")) {
			transform.Rotate (Vector3.back, 2, 0);
		}
	}
}
