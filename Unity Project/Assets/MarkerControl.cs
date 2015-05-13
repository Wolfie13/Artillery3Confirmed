using UnityEngine;
using System.Collections;

public class MarkerControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.Rotate (new Vector3 (0, Time.deltaTime * 40.0f, 0));
	}
}
