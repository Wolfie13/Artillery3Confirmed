/*
**	DragMeshFaces.cs
**
**	Copyright (c) 2007 Neil Carter <n.carter@nether.org.uk>
**
**	The latest version of this source code and the license under which
**	it has been released are both available from the following URL:
**
**		http://nether.homeip.net:8080/unity/
*/

using UnityEngine;
using System;
using System.Collections;

// THIS CLASS IS UNUSED.  I just left it here for reference purposes.

public class DragMeshFaces : MonoBehaviour
{
	public Transform m_Target;		// Temporary
	public float m_Radius = 1.0f;

	Mesh m_TargetMesh;
	int[] m_VertexIndices;
	Matrix4x4 m_PreviousTransform;

	void Start()
	{
		DateTime startTime = DateTime.Now;
		SelectFaces();
		Console.WriteLine("DragMeshFaces.SelectFaces() took {0}", DateTime.Now - startTime);

		m_PreviousTransform = transform.worldToLocalMatrix;
	}

	void Update()
	{
		Vector3[] vertices = m_TargetMesh.vertices;
		Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;

		int count = m_VertexIndices.Length;
		for(int index = 0; index < count; ++index)
		{
//			if(index >= count)
//				Console.WriteLine("Index {0} overflows m_VertexIndices.Count of {1}", index, m_VertexIndices.Length);

//			if((int)m_VertexIndices[index] >= vertices.Length)
//				Console.WriteLine("Vertex index {0} overflows vertices.Length of {1}", (int)m_VertexIndices[index], vertices.Length);

			int vertexIndex = (int)m_VertexIndices[index];
			Vector3 vertex = m_PreviousTransform.MultiplyPoint(vertices[vertexIndex]);
			vertices[vertexIndex] = worldToLocalMatrix.inverse.MultiplyPoint(vertex);
		}

		m_TargetMesh.vertices = vertices;
		m_TargetMesh.RecalculateBounds();

		m_PreviousTransform = worldToLocalMatrix;
	}

	void SelectFaces()
	{
		Vector3 position = transform.position;

		MeshFilter targetMeshFilter = (MeshFilter)m_Target.GetComponent(typeof(MeshFilter));
		m_TargetMesh = targetMeshFilter.mesh;

		Vector3[] vertices = m_TargetMesh.vertices;
		int[] triangles = m_TargetMesh.triangles;

		Hashtable vertexIndicesHash = new Hashtable();
		Hashtable sharedVertexHash = new Hashtable();

		// Find out which vertices are shared between multiple faces:

		for(int index = 0; index < triangles.Length; ++index)
		{
			int vertexIndex = triangles[index];
			if(sharedVertexHash.Contains(vertexIndex))
				sharedVertexHash[vertexIndex] = (int)sharedVertexHash[vertexIndex] + 1;
			else
				sharedVertexHash[vertexIndex] = 1;
		}

		// Record the triangles we want to move:

		for(int index = 0; index < triangles.Length - 3; index += 3)
		{
			int numVerticesInSphere = 0;
			for(int cornerIndex = index; cornerIndex < index + 3; ++cornerIndex)
			{
				// Only include vertices inside the sphere:

				if((vertices[triangles[cornerIndex]] - position).sqrMagnitude < m_Radius * m_Radius)
				{
					// Only include those vertices which aren't shared by multiple triangles:

					if((int)sharedVertexHash[triangles[cornerIndex]] == 1)
						++numVerticesInSphere;
				}
			}

			// Only include triangles for whome all of the vertices pass the above tests:

			if(numVerticesInSphere == 3)
			{
				for(int cornerIndex = index; cornerIndex < index + 3; ++cornerIndex)
					vertexIndicesHash[triangles[cornerIndex]] = 0;
			}
		}

		m_VertexIndices = new int[vertexIndicesHash.Count];
		vertexIndicesHash.Keys.CopyTo(m_VertexIndices, 0);

		Console.WriteLine("Selected {0} vertices of {1}", m_VertexIndices.Length, vertices.Length);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, m_Radius);
	}
}
