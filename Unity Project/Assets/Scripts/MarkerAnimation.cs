using UnityEngine;
using System.Collections;

public class MarkerAnimation : MonoBehaviour
{
	public float rotationSpeed = 90F;

	// Update is called once per frame
	void Update ()
	{
		// rotation
		transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime);
	}
}
