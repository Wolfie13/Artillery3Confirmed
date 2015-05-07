using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
	public GameObject[] projectileList;
	int muzzleSpeed = 40;

	public void Shoot (int projectileIndex)
	{
		GameObject instantiatedProjectile = Instantiate(projectileList[projectileIndex], transform.position, transform.rotation) as GameObject;
		instantiatedProjectile.rigidbody.velocity = transform.TransformDirection(new Vector3(0, muzzleSpeed, 0));
	}

	// Use this for initialization
	void Start ()
	{
	
	}

}
