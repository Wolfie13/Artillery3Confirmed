using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("projectile"))
	    {
			damageTerrain(col.gameObject.transform.position, 100);
			Destroy(col.gameObject);
		}
	}

	public const int HORIZONTAL_RES = 800;
	public const int VERTICAL_RES = 600;

	private static Color CLEAR = new Color(255, 255, 255, 255);


	//TODO: refactor to work in a 2radius area around pos only. For now, this will be fine.
	void damageTerrain(Vector3 pos, float radius)
	{
		for (int y = 0; y != VERTICAL_RES; y++) {
			//Set up the state machine used for the mesh generator.
			for (int x = 0; x != HORIZONTAL_RES; x++) {
				Vector3 vertPos = new Vector3(x, y, 0);
				float dist = Vector3.Distance(vertPos, pos);
				if (dist > radius) {
					return;
				}

				terrainMap.SetPixel(x, y, CLEAR);
			}
		}

		TerrainMeshGenerator generator = this.gameObject.GetComponent<TerrainMeshGenerator>();
		generator.UpdateMesh ();
	}

	public Texture2D terrainMap = null;
}
