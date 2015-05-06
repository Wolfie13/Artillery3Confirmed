using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainMeshGenerator : MonoBehaviour {

	public MeshFilter terrainMesh = null;
	public Texture2D terrainMap = null;
	// Use this for initialization
	void Start () {
		UpdateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	private const int HORIZONTAL_RES = 800;
	private const int VERTICAL_RES = 600;

	private Vector3[] start = {new Vector3(0.0f, 0.5f, 0.5f), new Vector3(0.0f, 0.5f, -0.5f), new Vector3(0.0f, -0.5f, 0.5f), new Vector3(0.0f, -0.5f, -0.5f)};

	private Vector3[] end = {new Vector3(0.0f, 0.5f, 0.5f), new Vector3(0.0f, 0.5f, -0.5f), new Vector3(0.0f, -0.5f, 0.5f), new Vector3(0.0f, -0.5f, -0.5f)};

	private static void AddWithOffset (List<Vector3> list, Vector3[] items, Vector3 offset)
	{
		foreach (Vector3 v in items)
		{
			list.Add(v + offset);
		}
	}

	private static void AddUVs (List<Vector2> list, Vector2 uv)
	{
		list.Add (uv);
		list.Add (uv);
		list.Add (uv);
		list.Add (uv);
	}

	private static void AddTriangles (List<int> tris, int lastPoint)
	{
		//[start/end, top/bottom, back/front]
		int ebb = lastPoint;
		int ebf = lastPoint - 1;
		int etb = lastPoint - 2;
		int etf = lastPoint - 3;
		int sbb = lastPoint - 4;
		int sbf = lastPoint - 5;
		int stb = lastPoint - 6;
		int stf = lastPoint - 7;

		//*//Front plane
		tris.Add (ebf);
		tris.Add (etf);
		tris.Add (stf);

		tris.Add (stf);
		tris.Add (sbf);
		tris.Add (ebf);//*/

		//*Top Plane
		tris.Add (stf);
		tris.Add (etf);
		tris.Add (etb);

		tris.Add (etb);
		tris.Add (stb);
		tris.Add (stf);
		//*/

		//*Bottom Plane
		tris.Add (ebb);
		tris.Add (ebf);
		tris.Add (sbf);

		tris.Add (sbf);
		tris.Add (sbb);
		tris.Add (ebb);
		//*/

		//*Start End Plane
		tris.Add (sbb);
		tris.Add (sbf);
		tris.Add (stf);

		tris.Add (stf);
		tris.Add (stb);
		tris.Add (sbb);
		//*/

		//*End End Plane
		tris.Add (etf);
		tris.Add (ebf);
		tris.Add (ebb);
		
		tris.Add (ebb);
		tris.Add (etb);
		tris.Add (etf);
		//*/

	}

	private static Vector2 GetUVForCoord(int x, int y)
	{
		return new Vector2((1f / HORIZONTAL_RES) * x, (1f / VERTICAL_RES) * y);
	}

	void OnDrawGizmosSelectedOff()
	{
		for (int y = 0; y != VERTICAL_RES; y++) {
			//Set up the state machine used for the mesh generator.
			for (int x = 0; x != HORIZONTAL_RES; x++) {
				Vector3 basePos = new Vector3(x, y, 0);
				Vector2 uv = GetUVForCoord(x, y);
				Color c = terrainMap.GetPixelBilinear(uv.x, uv.y);
				if (c.a > 0.1f)
				{
					Gizmos.color = new Color(uv.x, uv.y, 0);
					Gizmos.DrawCube(basePos, new Vector3(0.2f, 0.2f, 0.2f));
				}

			}
		}
	}

	private enum GeneratorState {RUNNING, CLEAR};
	void UpdateMesh()
	{
		Mesh mesh = new Mesh ();
		this.terrainMesh.mesh = mesh;
		//Set up our storage for mesh data.
		List<Vector3> verts = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Vector2> uvs = new List<Vector2>();

		for (int y = 0; y != VERTICAL_RES; y++) {
			//Set up the state machine used for the mesh generator.
			GeneratorState genState = GeneratorState.CLEAR;
			for (int x = 0; x != HORIZONTAL_RES; x++) {
				Vector3 basePos = new Vector3(x, y, 0);

				Vector2 uv = GetUVForCoord(x, y);
				Color c = terrainMap.GetPixelBilinear(uv.x, uv.y);

				if (genState == GeneratorState.RUNNING) {
					if (c.a < 0.1) {
						//Transition to the CLEAR state, build the end of this strip.
						AddWithOffset(verts, end, basePos);
						AddUVs(uvs, uv);
						AddTriangles(triangles, verts.Count - 1);
						genState = GeneratorState.CLEAR;
					} else {
						//Do nothing.
						continue;
					}
				}
				else if (genState == GeneratorState.CLEAR) {
					if (c.a < 0.1) {
						//We don't need to do anything if we're not making a strip.
						continue;
					} else {
						//Build the start of a strip.
						AddWithOffset(verts, start, basePos);
						AddUVs(uvs, uv);
						genState = GeneratorState.RUNNING;
					}
				}

			}

			if (genState == GeneratorState.RUNNING) {
				//Transition to the CLEAR state, build the end of this strip.
				AddWithOffset(verts, end, new Vector3(HORIZONTAL_RES, y, 0));
				AddUVs(uvs, GetUVForCoord(HORIZONTAL_RES, y));
				AddTriangles(triangles, verts.Count - 1);
				genState = GeneratorState.CLEAR;
			}
		}

		mesh.vertices = verts.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.uv = uvs.ToArray ();
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
	}
}
