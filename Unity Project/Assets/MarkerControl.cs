using UnityEngine;
using System.Collections;

public class MarkerControl : MonoBehaviour {

	[HideInInspector]
	public GameObject targetObject = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.targetObject) {
			this.gameObject.transform.position = this.targetObject.transform.position + new Vector3 (0, 7.5f, 0);
			this.gameObject.transform.Rotate (new Vector3 (0, Time.deltaTime * 40.0f, 0));
		}
	}
}
