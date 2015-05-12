using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainMeshGenerator : MonoBehaviour {

	private MeshFilter terrainMesh = null;
	private Texture2D terrainMap = null;
	private MeshCollider terrainCollider = null;
	private bool loaded = false;

	// Use this for initialization
	void Start () {
		this.terrainMesh = this.gameObject.AddComponent<MeshFilter> ();
		//Ew. ECS. Again. I even copy-pasted this from Terrain.cs
		Texture2D tempTex = (Texture2D) this.gameObject.GetComponent<MeshRenderer> ().materials [0].GetTexture(0);
		this.terrainMap = new Texture2D (tempTex.width, tempTex.height, TextureFormat.RGBA32, false);
		terrainMap.SetPixels32 (tempTex.GetPixels32 ());
		terrainMap.Apply ();

		this.terrainCollider = this.gameObject.AddComponent<MeshCollider> ();
		
		UpdateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool isLoaded()
	{
		return this.loaded;
	}

	public Texture2D getTerrainMapInstance()
	{
		return terrainMap;
	}

	private const int HORIZONTAL_RES = Terrain.HORIZONTAL_RES;
	private const int VERTICAL_RES = Terrain.VERTICAL_RES;

	private const float TERRAIN_SCALE = 0.1f;
	
	private Vector3[] start = {new Vector3(0.0f, 0.5f * TERRAIN_SCALE, 5f), new Vector3(0.0f, 0.5f * TERRAIN_SCALE, -5f), new Vector3(0.0f, -0.5f * TERRAIN_SCALE, 5f), new Vector3(0.0f, -0.5f * TERRAIN_SCALE, -5f)};

	private Vector3[] end = {new Vector3(0.0f, 0.5f * TERRAIN_SCALE, 5f), new Vector3(0.0f, 0.5f * TERRAIN_SCALE, -5f), new Vector3(0.0f, -0.5f * TERRAIN_SCALE, 5f), new Vector3(0.0f, -0.5f * TERRAIN_SCALE, -5f)};

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
	
	public static Vector3 WorldToTerrain(Vector3 world)
	{
		return new Vector3(world.x * TERRAIN_SCALE, world.y * TERRAIN_SCALE, 0);
	}
	
	public static Vector3 TerrainToWorld(Vector3 terrain)
	{
		return new Vector3(terrain.x / TERRAIN_SCALE, terrain.y / TERRAIN_SCALE, 0);
	}

	private static Vector2 GetUVForCoord(int x, int y)
	{
		return new Vector2((1f / HORIZONTAL_RES) * x, (1f / VERTICAL_RES) * y);
	}

	public void UpdateMesh()
	{
		StartCoroutine(UpdateMeshInt ());
		//UpdateMeshInt ();
	}

	private enum GeneratorState {RUNNING, CLEAR};
	private IEnumerator UpdateMeshInt()
	{
		float t = Time.realtimeSinceStartup;
		Mesh mesh = new Mesh ();
		//Set up our storage for mesh data.
		List<Vector3> verts = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Vector2> uvs = new List<Vector2>();

		for (int y = 0; y != VERTICAL_RES; y++) {
			//Set up the state machine used for the mesh generator.
			GeneratorState genState = GeneratorState.CLEAR;
			for (int x = 0; x != HORIZONTAL_RES; x++) {
				Vector3 basePos = WorldToTerrain(new Vector3(x, y, 0));

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
				AddWithOffset(verts, end, WorldToTerrain(new Vector3(HORIZONTAL_RES, y, 0)));
				AddUVs(uvs, GetUVForCoord(HORIZONTAL_RES, y));
				AddTriangles(triangles, verts.Count - 1);
				genState = GeneratorState.CLEAR;
			}
			if (y % 30 == 0)
			{
				yield return new WaitForEndOfFrame();
			}
		}
		mesh.vertices = verts.ToArray ();
		mesh.triangles = triangles.ToArray ();
		mesh.uv = uvs.ToArray ();
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		this.terrainCollider.sharedMesh = mesh;
		this.terrainMesh.mesh = mesh;
		this.loaded = true;
		Debug.Log ("Time To Regen Mesh: " + (Time.realtimeSinceStartup - t).ToString());
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
}
