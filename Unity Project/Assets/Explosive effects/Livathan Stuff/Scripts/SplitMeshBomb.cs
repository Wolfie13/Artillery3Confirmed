/*
**	SplitMeshBomb.cs
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

public class SplitMeshBomb : MonoBehaviour
{
	public Transform m_Target;					// Temporary
	public GameObject m_DebrisPrefab;			// A prefab with a Rigidbody and whatever else you want your debris to carry
	public Material m_DebrisMaterial;			// Material to be applied to the debris meshes, or null if you want the existing material

	public float m_InitialChunkSize = 1.0f;

	public bool m_DestroyOriginal = true;		// Destroy the original mesh's GameObject
	public int m_TriangleBatchSize = 200;		// Lower numbers mean the processing takes longer overall, but slows the frame rate less
	public float m_YieldDuration = 0.05f;

	MeshComposer[] m_MeshComposers;
	Bounds[] m_BoundsList;
	bool m_IsReadyToBreakUp = false;



	/*
	**	The MeshArrays class is used to cache the arrays returns from the various properties of
	**	a Mesh object, since repeatedly calling those properties incurs significant overhead.
	*/
	class MeshArrays
	{
		public Vector3[] vertices;
		public Vector3[] normals;
//		public Vector4[] tangents;
		public Vector2[] uv;
//		public Vector2[] uv2;
//		public Color[] colours;
		public int[] triangles;

		public MeshArrays(Mesh sourceMesh)
		{
			vertices = sourceMesh.vertices;
			normals = sourceMesh.normals;
			uv = sourceMesh.uv;
			triangles = sourceMesh.triangles;
		}
	}



	/*
	**	The MeshComposer class is used to create a new Mesh object from a collection of triangles
	**	chosen from an existing Mesh object.
	*/
	class MeshComposer
	{
		public ArrayList vertices = new ArrayList();
		public ArrayList normals = new ArrayList();
		public ArrayList uv = new ArrayList();
		public ArrayList triangles = new ArrayList();

		int nextIndex = 0;

		public void AddTriangle(MeshArrays meshArrays, int firstVertexIndex)
		{
			// Make a copy of all the information for a given triangle in a given mesh:

			for(int index = firstVertexIndex; index < firstVertexIndex + 3; ++index)
			{
				int dataIndex = meshArrays.triangles[index];

				vertices.Add(meshArrays.vertices[dataIndex]);
				normals.Add(meshArrays.normals[dataIndex]);
				uv.Add(meshArrays.uv[dataIndex]);
				triangles.Add(nextIndex++);
			}
		}

		public Mesh ComposeMesh()
		{
			// Create a real Mesh object based upon the information that has been added to our ArrayLists:

			Mesh mesh = new Mesh();
			mesh.name = "Debris (" + triangles.Count / 3 + " triangles)";

			mesh.vertices = (Vector3[])vertices.ToArray(typeof(Vector3));

			if(normals != null && normals.Count > 0)
				mesh.normals = (Vector3[])normals.ToArray(typeof(Vector3));

			if(uv != null && uv.Count > 0)
				mesh.uv = (Vector2[])uv.ToArray(typeof(Vector2));

			mesh.triangles = (int[])triangles.ToArray(typeof(int));

			return mesh;
		}

		public bool isEmpty {get {return (triangles.Count == 0);}}
	}



	// Public methods:

	public void PrepareToBreakUp()
	{
		StartCoroutine("SplitMesh");
	}

	public void BreakUp()
	{
		if(!isReadyToBreakUp)
		{
			Debug.Log("SplitMeshBomb.BreakUp() called before PrepareToBreakUp().");
			Debug.Break();
		}

		if(m_DestroyOriginal)
			Destroy(m_Target.gameObject);

		// Convert the m_MeshComposers into real GameObjects:

		for(int index = 0; index < m_MeshComposers.Length; ++index)
		{
			if(!m_MeshComposers[index].isEmpty)
				BuildDebris(m_Target, m_MeshComposers[index].ComposeMesh(), m_BoundsList[index]);
		}
	}

	public void BreakUpWhenReady()
	{
		StartCoroutine("DeferredBreakUp");
	}

	public bool isReadyToBreakUp {get {return m_IsReadyToBreakUp;}}



	// Private methods:

	IEnumerator SplitMesh()
	{
		DateTime startTime = DateTime.Now;

		Transform original = m_Target;
		MeshFilter meshFilter = (MeshFilter)m_Target.GetComponent(typeof(MeshFilter));
		Mesh sourceMesh = meshFilter.mesh;

		// Get the existing material if an alternative wasn't specified:

		if(m_DebrisMaterial == null)
		{
			MeshRenderer meshRenderer = (MeshRenderer)m_Target.GetComponent(typeof(MeshRenderer));
			m_DebrisMaterial = meshRenderer.material;
		}

		// Figure out how we're going to break up the mesh:

		m_BoundsList = ChooseDebrisRegions(original, sourceMesh);

		// Create some m_MeshComposers, which are used to collate triangle data:

		m_MeshComposers = new MeshComposer[m_BoundsList.Length];
		for(int index = 0; index < m_MeshComposers.Length; ++index)
			m_MeshComposers[index] = new MeshComposer();

		MeshArrays meshArrays = new MeshArrays(sourceMesh);

		// For each triangle, find out which debris regions it lies within and add it to the appropriate
		// MeshComposer object:

		for(int index = 0; index < meshArrays.triangles.Length; index += 3)
		{
			for(int boundsIndex = 0; boundsIndex < m_BoundsList.Length; ++boundsIndex)
			{
				int numVerticesInBounds = 0;
				for(int cornerIndex = index; cornerIndex < index + 3; ++cornerIndex)
				{
					if(m_BoundsList[boundsIndex].Contains(meshArrays.vertices[meshArrays.triangles[cornerIndex]]))
						++numVerticesInBounds;
				}

				if(numVerticesInBounds == 3)
				{
					m_MeshComposers[boundsIndex].AddTriangle(meshArrays, index);
					break;
				}
			}

			// Yield a bit to allow processing to take place over a few frames:

			if(index % m_TriangleBatchSize * 3 == 0)
			{
//				Console.WriteLine("Yielding, {0} triangles still to go.", meshArrays.triangles.Length - index);
				yield return new WaitForSeconds(m_YieldDuration);
			}
		}

		m_IsReadyToBreakUp = true;

		Console.WriteLine("SplitMeshBomb breakup took {0}", DateTime.Now - startTime);
	}

	Bounds[] ChooseDebrisRegions(Transform original, Mesh sourceMesh)
	{
		ArrayList m_BoundsList = new ArrayList();
		Bounds overallBounds = sourceMesh.bounds;

		// Choose a set of equal divisions for the new mesh parts:

		for(float z = overallBounds.min.z - m_InitialChunkSize * UnityEngine.Random.value;
			z < overallBounds.max.z;
			z += m_InitialChunkSize) // * 0.8f + (UnityEngine.Random.value * 0.2f))
		{
			for(float y = overallBounds.min.y - m_InitialChunkSize * UnityEngine.Random.value;
				y < overallBounds.max.y;
				y += m_InitialChunkSize) // * 0.8f + (UnityEngine.Random.value * 0.2f))
			{
				for(float x = overallBounds.min.x - m_InitialChunkSize * UnityEngine.Random.value;
					x < overallBounds.max.x;
					x += m_InitialChunkSize) // * 0.8f + (UnityEngine.Random.value * 0.2f))
				{
					Bounds bounds = new Bounds(new Vector3(x, y, z), Vector3.one * m_InitialChunkSize * 1.2f);
					Console.WriteLine("Adding new bounds ({0}, {1}, {2}): {3}", x, y, z, bounds);
					m_BoundsList.Add(bounds);
				}
			}
		}

		// Randomly subdivide some of them:

		ArrayList subdividedm_BoundsList = new ArrayList();
		foreach(Bounds bounds in m_BoundsList)
			SubdivideBounds(bounds, subdividedm_BoundsList);

		return (Bounds[])subdividedm_BoundsList.ToArray(typeof(Bounds));
	}

	void SubdivideBounds(Bounds bounds, ArrayList m_BoundsList)
	{
		int index = UnityEngine.Random.Range(-1, 3);
		if(index >= 0)
		{
			// The following code depends, evilly, upon the random number being used to index the vector's axes:

			Vector3 offset = Vector3.zero;
			Vector3 size = new Vector3(bounds.size.x, bounds.size.y, bounds.size.z);
			offset[index] = bounds.size[index] * 0.25f;
			size[index] = bounds.size[index] * 0.5f;

			Console.WriteLine("Subdividing bounds {0} in the {1} axis", bounds, (index == 0 ? "X" : (index == 1 ? "Y" : "Z")));

			m_BoundsList.Remove(bounds);
			m_BoundsList.Add(new Bounds(bounds.center - offset, size));
			m_BoundsList.Add(new Bounds(bounds.center + offset, size));
		}
		else
			m_BoundsList.Add(bounds);
	}

	GameObject BuildDebris(Transform original, Mesh mesh, Bounds bounds)
	{
		// Create a debris object whose position is identical to the original mesh (that is, probably grossly
		// offset from the centroid of its faces):

		GameObject debris = new GameObject("Mesh");
		debris.transform.position = original.position;
		debris.transform.rotation = original.rotation;

		MeshFilter meshFilter = (MeshFilter)debris.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = mesh;

		MeshRenderer meshRenderer = (MeshRenderer)debris.AddComponent(typeof(MeshRenderer));
		meshRenderer.material = m_DebrisMaterial;

		// Create a wrapper object which will cancel out the offset of the debris mesh.  This is a lot cheaper
		// than transforming every point while processing the mesh:

		GameObject wrapper = (GameObject)Instantiate(m_DebrisPrefab, meshRenderer.bounds.center, original.rotation);
		wrapper.name = original.name + " " + mesh.name;
		debris.transform.parent = wrapper.transform;

		// Make its motion more or less match that of the original object (torque cannot be taken into account
		// here as far as I know):

		if(original.rigidbody)
			wrapper.rigidbody.velocity = original.rigidbody.GetPointVelocity(wrapper.rigidbody.transform.position);

		// Some random torque to make it spin slowly:

		wrapper.rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * UnityEngine.Random.value * 2.0f);

		// And a collider, which is somewhat smaller than the original mesh to avoid a bunch of immediate
		// collisions and to allow the debris to 'collapse' a bit:

		BoxCollider boxCollider = (BoxCollider)wrapper.AddComponent(typeof(BoxCollider));
		boxCollider.center = mesh.bounds.center + debris.transform.localPosition;
		boxCollider.size = mesh.bounds.size * 0.7f;

		return debris;
	}

	IEnumerator DeferredBreakUp()
	{
		while(!isReadyToBreakUp)
			yield return 0;

		BreakUp();
	}
}
