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

	private const int HORIZONTAL_RES = 128;
	private const int VERTICAL_RES = 128;
	private enum GeneratorState {RUNNING, CLEAR};
	void UpdateMesh()
	{
		List<Vector3> verts = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Vector2> uvs = new List<Vector2>();
		GeneratorState state = GeneratorState.CLEAR;
		int startPoint = -1;
		for (int x = 0; x != HORIZONTAL_RES; x++) {
			GeneratorState genState = GeneratorState.CLEAR;
			for (int y = 0; x != VERTICAL_RES; y++) {
				Vector2 uv = new Vector2((1 / HORIZONTAL_RES) * x, (1 / VERTICAL_RES) * y);
				Color c = terrainMap.GetPixelBilinear(uv.x, uv.y);
				//We don't need to do anything if we're not making a strip.

				if (genState == GeneratorState.RUNNING) {
					if (c.a == 0.0) {
						//Transition to the CLEAR state, build the end of this strip.
					}
				}
				else {
					if (c.a == 0.0) {
						continue;
					}
				}

			}
		}
	}
}
