/*
**	ParticleEmitterStopper.cs
**
**	Copyright (c) 2005 Neil Carter <n.carter@nether.org.uk>
**
**	The latest version of this source code and the license under which
**	it has been released are both available from the following URL:
**
**		http://nether.homeip.net:8080/unity/
*/

using UnityEngine;
using System.Collections;

public class ParticleEmitterStopper : MonoBehaviour
{
	public float duration = 0.1f;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(duration);
		particleEmitter.emit = false;
	}
}
