using UnityEngine;
using System.Collections;

public class TankGunRotation : MonoBehaviour
{
	public float maxAngle;
	public float speedRotation = 2;
	float angle = 0;

	void Start ()
	{
		transform.Rotate (Vector3.forward, 45, 0);
	}

	public void Rotate (int dir)
	{
		transform.Rotate (Vector3.forward * -1, speedRotation, 0);
	}
}

