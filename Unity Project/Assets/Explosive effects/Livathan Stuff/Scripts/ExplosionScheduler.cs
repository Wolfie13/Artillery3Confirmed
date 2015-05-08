/*
**	ExplosionScheduler.cs
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

public class ExplosionScheduler : MonoBehaviour
{
	public float m_InitialDelay = 0.5f;
	public GameObject m_HugeExplosionPrefab;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(m_InitialDelay);

		SplitMeshBomb splitMeshBomb = (SplitMeshBomb)GetComponent(typeof(SplitMeshBomb));
		splitMeshBomb.PrepareToBreakUp();
		splitMeshBomb.BreakUpWhenReady();
		transform.parent = null;

		if(m_HugeExplosionPrefab)
			Instantiate(m_HugeExplosionPrefab, transform.position, Quaternion.identity);
	}
}
