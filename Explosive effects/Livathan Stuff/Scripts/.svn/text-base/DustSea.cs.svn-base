/*
**	DustSea.cs
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

public class DustSea : MonoBehaviour
{
	public float m_DragIncrement = 0.5f;
	public float m_AngularDragIncrement = 0.5f;
	public float m_ResetDrag = 0.0f;
	public float m_ResetAngularDrag = 0.05f;

	void OnTriggerStay(Collider other)
	{
		Rigidbody r = other.rigidbody;
		if(r)
		{
			r.drag += m_DragIncrement;
			r.angularDrag += m_AngularDragIncrement;
		}
	}

	void OnTriggerExit(Collider other)
	{
		// If the object escapes altogether, set its drag back to some reasonable value to stop it from
		// floating around like a giant piece of polystyrene:

		Rigidbody r = other.rigidbody;
		if(r)
		{
			r.drag = m_ResetDrag;
			r.angularDrag = m_ResetAngularDrag;
		}
	}
}
