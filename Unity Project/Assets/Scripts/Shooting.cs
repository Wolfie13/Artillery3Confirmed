using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour {

	public GameObject FirePoint;
	public GameObject Weapon;
	public int firepower;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("x")) {
			GameObject progectile = (GameObject)Instantiate (Weapon, FirePoint.transform.position, FirePoint.transform.rotation);
			progectile.rigidbody.AddForce (FirePoint.transform.forward * firepower);
		}
	}
}
