using UnityEngine;
using System.Collections;

public class TankGunRotation : MonoBehaviour
{
	//TODO: Fix

	public float maxAngle;
	public float speedRotation = 2;
	float angle = 45;

	void Start ()
	{
		transform.Rotate (Vector3.forward, 45, 0);
	}

	public void Rotate (int dir)
	{
		float newAngle = angle + speedRotation * Time.deltaTime * dir;
		if (newAngle < maxAngle && newAngle > -maxAngle)
		{
			transform.Rotate (Vector3.forward * dir, speedRotation * Time.deltaTime, 0);

			angle += speedRotation * dir * Time.deltaTime;
		}
	}
}

