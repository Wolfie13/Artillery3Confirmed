/*
**	CameraShake.cs
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

public class CameraShake : MonoBehaviour
{
	public float m_MaximumMagnitude = 1.0f;
	public float m_BaseTranslationFactor = 0.01f;
	public float m_BaseRotationFactor = 0.01f;
	public float m_DecreaseRate = 1.0f;

	float m_CurrentMagnitude;
	Vector3 m_OriginalPosition;
	Quaternion m_OriginalOrientation;

	static CameraShake s_CameraShake;

	void Start()
	{
		s_CameraShake = this;

		m_OriginalPosition = transform.position;
		m_OriginalOrientation = transform.rotation;
	}

	void OnApplicationQuit()
	{
		s_CameraShake = null;
	}

	void LateUpdate()
	{
		if(m_CurrentMagnitude > 0.0f)
		{
			transform.position = m_OriginalPosition + Random.insideUnitSphere * m_BaseTranslationFactor * m_CurrentMagnitude;
			transform.rotation = Quaternion.Lerp(m_OriginalOrientation, Random.rotation, m_BaseRotationFactor * m_CurrentMagnitude);
			m_CurrentMagnitude -= m_DecreaseRate * Time.deltaTime;
		}
		else
			m_CurrentMagnitude = 0.0f;
	}

	public static void IncreaseMagnitude(float amount)
	{
		s_CameraShake.m_CurrentMagnitude += amount;
		s_CameraShake.m_CurrentMagnitude = Mathf.Min(s_CameraShake.m_CurrentMagnitude, s_CameraShake.m_MaximumMagnitude);
	}
}
