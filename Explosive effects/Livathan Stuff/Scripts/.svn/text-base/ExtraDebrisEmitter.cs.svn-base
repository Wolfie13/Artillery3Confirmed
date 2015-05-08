/*
**	ExtraDebrisEmitter.cs
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

public class ExtraDebrisEmitter : MonoBehaviour
{
	public GameObject[] m_DebrisPrefabs;
	public int m_MinimumPieces;
	public int m_MaximumPieces;
	public Vector3 m_BoundsMinimum;
	public Vector3 m_BoundsMaximum;
	public int m_Bias = 0;

	void Start()
	{
		int numPieces = System.Math.Max(Random.Range(m_MinimumPieces, m_MaximumPieces + 1) + m_Bias, 0);
		for(int index = 0; index < numPieces; ++index)
		{
			Vector3 direction = new Vector3(
				Random.Range(m_BoundsMinimum.x, m_BoundsMaximum.x),
				Random.Range(m_BoundsMinimum.y, m_BoundsMaximum.y),
				Random.Range(m_BoundsMinimum.z, m_BoundsMaximum.z));

			GameObject piece = (GameObject)Instantiate(m_DebrisPrefabs[Random.Range(0, m_DebrisPrefabs.Length)],
				transform.position + direction, Random.rotation);

			piece.transform.localScale = Vector3.one * (0.8f + (Random.value * 0.4f));
			piece.rigidbody.AddForce((direction).normalized * 2.0f, ForceMode.Impulse);
			piece.rigidbody.AddTorque(Random.insideUnitSphere * 3.0f, ForceMode.Impulse);

			Destroy(piece, Random.value * 2.0f + 1.0f);
		}
	}
}
