/*
**	HugeExplosion.cs
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

public class HugeExplosion : MonoBehaviour
{
	public float m_Duration = 3.0f;
	public float m_ParticleInterval = 0.25f;
	public float m_Velocity = 1.0f;

	ParticleEmitter m_Emitter;
	float m_StartTime;

	void Start()
	{
		light.enabled = true;
		m_Emitter = particleEmitter;
		m_StartTime = Time.time;

		StartCoroutine("Explode");
	}

	void Update()
	{
		if(light.enabled)
		{
			float factor = Mathf.PingPong(Time.time - m_StartTime, m_Duration * 0.5f);
			light.color = Color.white * factor;

			CameraShake.IncreaseMagnitude(factor * 0.1f);
		}

		CameraShake.IncreaseMagnitude(0.03f);
	}

	IEnumerator Explode()
	{
		do
		{
			Vector3 position = transform.position;
			Vector3 direction = (Camera.main.transform.position - position).normalized * m_Velocity;
			m_Emitter.Emit(position, direction,
				Random.Range(m_Emitter.minSize, m_Emitter.maxSize),
				Random.Range(m_Emitter.minEnergy, m_Emitter.maxEnergy),
				new Color(1.0f, 1.0f, 1.0f, 0.0f));

			yield return new WaitForSeconds(m_ParticleInterval);
		}
		while(Time.time - m_StartTime < m_Duration);

		light.enabled = false;

		// And autodestruction will remove the object when the particles expire.
	}
}
