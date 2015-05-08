/*
**	ContactBurst.cs
**
**	Copyright (c) 2007 Neil Carter <n.carter@nether.org.uk>
**
**	The latest version of this source code and the license under which
**	it has been released are both available from the following URL:
**
**		http://nether.homeip.net:8080/unity/
*/

using UnityEngine;
using System.Collections;

public class ContactBurst : MonoBehaviour
{
	public GameObject m_BurstPrefab;
	public GameObject m_FirePrefab;

	public float m_ExtraReboundForce = 0.02f;
	public float m_FireProbability = 0.05f;

	void OnCollisionEnter(Collision collision)
	{
		foreach(ContactPoint contactPoint in collision.contacts)
		{
			if(m_BurstPrefab)
				Instantiate(m_BurstPrefab, contactPoint.point, Quaternion.identity);

			Rigidbody thisRigidbody = contactPoint.thisCollider.rigidbody;
			if(m_ExtraReboundForce != 0.0f && thisRigidbody != null)
			{
				if(contactPoint.otherCollider.name != "Ground Solid")
				{
					thisRigidbody.AddForceAtPosition(contactPoint.normal * m_ExtraReboundForce, contactPoint.point, ForceMode.Impulse);

					if(m_FirePrefab != null && Random.value < m_FireProbability)
					{
						GameObject fire = (GameObject)Instantiate(m_FirePrefab, contactPoint.point, Quaternion.identity);
						fire.transform.parent = contactPoint.thisCollider.transform;
					}
				}
				else
					CameraShake.IncreaseMagnitude(0.05f);
			}
		}
	}
}
