using UnityEngine;
using System.Collections;

public class ButtonsStuff : MonoBehaviour {

	public Material one;
	public Material two;
	public Renderer rend;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseEnter() {
		rend.material = two;
	}

	void OnMouseExit() {
		rend.material = one;
	}

}
