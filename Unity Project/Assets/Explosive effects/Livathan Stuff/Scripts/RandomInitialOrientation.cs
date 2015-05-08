/*
**	RandomInitialOrientation.cs
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

public class RandomInitialOrientation : MonoBehaviour
{
	public float m_StandoffDistance = 10.0f;	// Move back a bit so we hit the middle of the view

	void Awake()
	{
		transform.rotation = Random.rotation;
		transform.position += Vector3.Scale(transform.forward * m_StandoffDistance, new Vector3(1.0f, 0.0f, 1.0f));
	}
}
